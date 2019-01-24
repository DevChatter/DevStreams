
Vue.component('vue-multiselect', window.VueMultiselect.default);

let app = new Vue({
    components: {
        Multiselect: window.VueMultiselect.default
    },
    el: "#app",
    data: {
        selectedCountry: document.getElementById('country').value,
        selectedTimeZone: document.getElementById('timeZone').value,
        selectedTags: [],
        timeZoneOptions: [],
        tags: [],
        isLoadingTags: false,
    },
    computed: {
        tagIds() {
            return this.selectedTags.map(t => t.id).join(',');
        }
    },
    watch: {
        selectedCountry: function (value, previous) {
            const timeZoneSelect = document.getElementById('timeZone');
            while (timeZoneSelect.options.length > 0) {
                timeZoneSelect.remove(0);
            }
            this.timeZoneOptions = [];
            this.selectedTimeZone = null;
            this.fetchTimeZones(value);
        }
    },
    methods: {
        fetchTimeZones: function (countryCode) {
            axios.get(`/api/TimeZones/?CountryCode=${countryCode}`)
                .then(response => {
                    this.timeZoneOptions = Object.keys(response.data)
                        .map(key => ({ key: key, value: response.data[key] }));
                    this.selectedTimeZone = this.timeZoneOptions[0].key;
                }, error => {
                    console.log(error.statusText);
                });
        },
        tagSearch: function (filter) {
            axios.get(`/api/Tags/?filter=${encodeURIComponent(filter)}`)
                .then(response => {
                    this.tags = response.data;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
        }
    }
});
