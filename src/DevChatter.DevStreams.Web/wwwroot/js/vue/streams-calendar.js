let calendar = new Vue({
    el: "#cal",
    data: {
        selectedDate: moment().format('YYYY-MM-DD'),
        selectedCountry: document.getElementById('country').value,
        selectedTimeZone: document.getElementById('timeZone').value,
        timeZoneOptions: [],
        events: []
    },
    watch: {
        selectedCountry: function (value, previous) {
            const timeZoneSelect = document.getElementById('timeZone');
            while (timeZoneSelect.options.length > 0) {
                timeZoneSelect.remove(0);
            }
            this.timeZoneOptions = [];
            this.selectedTimeZone = null;
            this.fetchTimeZones();
        },
        selectedTimeZone: function (value, previous) {
            this.fetchEvents();
        },
        selectedDate: function(value, previous) {
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
                    this.selectedTimeZone = this.timeZoneOptions[0].key;
                }, error => {
                    console.log(error.statusText);
                });
        },
        fetchEvents: function () {
            if (this.selectedTimeZone && this.selectedDate) {
                const timeZoneId = this.selectedTimeZone;
                const localDate = moment(this.selectedDate).format('YYYY-MM-DD');
                axios.get(`/api/Events?timeZoneId=${timeZoneId}&localDateTime=${localDate}`)
                    .then(response => {
                            this.events = response.data;
                        },
                        error => {
                            console.log(error.statusText);
                        });
            }
        }
    },
    mounted: function () {
        this.fetchTimeZones();
    }
});
