let app = new Vue({
    el: "#app",
    data: {
        selectedCountry: document.getElementById('country').value,
        selectedTimeZone: document.getElementById('timeZone').value,
        timeZoneOptions: [],
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
                }, error => {
                    console.log(error.statusText);
                });
        }
    }
});
