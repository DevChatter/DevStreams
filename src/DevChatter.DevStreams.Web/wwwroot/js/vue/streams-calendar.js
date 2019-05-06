Vue.component('tag-selector', {
    data: function () {
        return {
            selectedTags: [],
            tags: [],
            isLoadingTags: false,
            showMoreTags: false
        };
    },
    props: {
        taggedItems: { type: Array, required: false, default: null },
        showCount: { type: Boolean, required: true }
    },
    template: '#tag-selector-template',
    mounted() {
        this.tagSearch('');
    },
    computed: {
        availableTags: function () {
            return this.tags.filter(tag => this.selectedTags.indexOf(tag) === -1);
        }
    },
    methods: {
        tagSearch: function (filter) {
            axios.get(`/api/Tags/?filter=${encodeURIComponent(filter)}`)
                .then(response => {
                    this.tags = response.data;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
            if (this.showCount) {
                this.tags
                    .forEach(
                        tag => tag.count = this.taggedItems.filter(
                            (x) => x.tags.map(
                                t => t.name).indexOf(tag.name) > -1).length);
            }
        },
        showMoreLessTags: function () {
            this.showMoreTags = !this.showMoreTags;
        },
        clickTag: function (tag) {
            var index = this.selectedTags.indexOf(tag);
            if (index > -1) {
                this.selectedTags.splice(index, 1);
            } else {
                this.selectedTags.push(tag);
            }
            this.$emit('selection-changed', this.selectedTags);
        },
        displayTag: function (tag) {
            let text = tag.name;
            if (this.showCount) {
                text += ` (${tag.count})`;
            }
            return text;
        }
    }
});

let calendar = new Vue({
    el: "#cal",
    data: {
        model: {
            selectedTags: [],
            selectedDate: moment().format('YYYY-MM-DD'),
            selectedTimeZone: document.getElementById('timeZone').value
        },
        selectedCountry: document.getElementById('country').value,
        timeZoneOptions: [],
        events: [],
    },
    computed: {
        selectedDate() {
            return this.model.selectedDate;
        },
        includedTags() {
            return this.model.selectedTags;
        },
        selectedTimeZone() {
            return this.model.selectedTimeZone;
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
        }
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
        tagSelectionChanged: function (tags) {
            this.model.selectedTags = tags;
        }
    },
    mounted: function () {
        this.fetchTimeZones();
    }
});
