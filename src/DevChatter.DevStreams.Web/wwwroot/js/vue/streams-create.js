let app = new Vue({
    el: "#app",
    data: {
        selectedCountry: null,
        selectedTimeZone: null,
        timeZoneOptions: [],
    },
    watch: {
        selectedCountry: function (value, previous) {
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
                }, error => {
                    console.log(error.statusText);
                });
        }
    }
});
