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
                const localDate = moment(this.model.selectedDate).format('YYYY-MM-DD');

                axios.post(`/api/Events/`, this.model)
                    .then(response => {
                        this.events = response.data;
                    })
                    .catch(error => {
                        debugger;
                        console.log(error.statusText);
                    });
            }
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
    },
    mounted: function () {
        this.fetchTimeZones();
    }
});
