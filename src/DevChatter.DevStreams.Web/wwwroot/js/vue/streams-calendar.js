Vue.component('vue-multiselect', window.VueMultiselect.default);

let calendar = new Vue({
    components: {
        Multiselect: window.VueMultiselect.default
    },
    el: "#cal",
    data: {
        model: {
            includedTags: [],
            selectedDate: moment().format('YYYY-MM-DD'),
            selectedTimeZone: document.getElementById('timeZone').value,
        },

        selectedCountry: document.getElementById('country').value,
        timeZoneOptions: [],
        events: [],
        tags: [],
        isLoadingTags: false,
        showMoreTags: false
    },
    computed: {
        selectedDate() {
            return this.model.selectedDate;
        },
        selectedTimeZone() {
            return this.model.selectedTimeZone;
        },
        includedTags() {
            return this.model.includedTags;
        },
        availableTags: function () {
            const selectedTags = this.model.includedTags;
            return this.tags
                .filter(tag => selectedTags.indexOf(tag) === -1);
        }
    },
    watch: {
        selectedCountry: function (value, previous) {
            const timeZoneSelect = document.getElementById('timeZone');
            while (timeZoneSelect.options.length > 0) {
                timeZoneSelect.remove(0);
            }
            this.timeZoneOptions = [];
            this.model.selectedTimeZone = null;
            this.fetchTimeZones();
        },
        includedTags() {
            this.fetchEvents();
        },
        selectedDate() {
            this.fetchEvents();
        },
        selectedTimeZone() {
            this.fetchEvents();
        },
    },
    methods: {
        fetchTimeZones: function () {
            const countryCode = this.selectedCountry;
            axios.get(`/api/TimeZones/?CountryCode=${countryCode}`)
                .then(response => {
                    this.timeZoneOptions = Object.keys(response.data)
                        .map(key => ({ key: key, value: response.data[key] }));
                    this.model.selectedTimeZone = this.timeZoneOptions[0].key;
                }, error => {
                    console.log(error.statusText);
                });
        },
        fetchEvents: function () {
            if (this.model.selectedTimeZone && this.model.selectedDate) {
                axios.post(`/api/Events/`, this.model)
                    .then(response => {
                        this.events = response.data.map(this.convertDataToLocal);
                    })
                    .catch(error => {
                        console.log(error.statusText);
                    });
            }
        },
        convertDataToLocal: function (event) {
            let startMoment = new moment(event.utcStartTime);
            let endMoment = new moment(event.utcEndTime);
            return {
                channelName: event.channelName,
                localStartTime: startMoment.calendar(),
                localEndTime: endMoment.calendar(),
                streamLength: moment.duration(endMoment.diff(startMoment)).humanize()
            };
        },
        tagSearch: function (filter) {
            axios.get(`/api/Tags/?filter=${encodeURIComponent(filter)}`)
                .then(response => {
                    this.tags = response.data;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
        },
        showMoreLessTags: function () {
            this.showMoreTags = !this.showMoreTags;
        },
        clickTag: function (tag) {
            var index = this.model.includedTags.indexOf(tag);
            if (index > -1) {
                this.model.includedTags.splice(index, 1);
            } else {
                this.model.includedTags.push(tag);
            }
        }

    },
    mounted: function () {
        this.fetchTimeZones();
        this.tagSearch('');
    }
});
