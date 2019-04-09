Vue.component('vue-multiselect', window.VueMultiselect.default);

let app = new Vue({
    components: {
        Multiselect: window.VueMultiselect.default
    },
    el: "#app",
    data: {
        channelId: document.getElementById('channelId').value,
        model: null,
        selectedTimeZone: null,
        selectedTags: [],
        timeZoneOptions: [],
        tags: [],
        isLoadingTags: false,
    },
    mounted() {
        axios.get(`/api/Channels/${this.channelId}`)
        .then(response => {
            this.model = response.data;
        })
        .catch(error => {
            console.log(error.statusText);
        });
    },
    computed: {
        selectedCountry() {
            if (this.model === null){
                return null;
            }
            return this.model.countryCode;
        }
    },
    watch: {
        selectedCountry: function (value, previous) {
            // const timeZoneSelect = this.model.timeZoneId;
            //  document.getElementById('timeZone');
            // while (timeZoneSelect.options.length > 0) {
            //     timeZoneSelect.remove(0);
            // }
            this.timeZoneOptions = [];
           // this.selectedTimeZone = null;
            this.fetchTimeZones(value);
        }
    },
    methods: {
        fetchTimeZones: function (countryCode) {
            axios.get(`/api/TimeZones/?CountryCode=${countryCode}`)
                .then(response => {
                    this.timeZoneOptions = Object.keys(response.data)
                        .map(key => ({ key: key, value: response.data[key] }));

                    var currentIndex = this.timeZoneOptions
                        .findIndex(t => t.key === this.model.timeZoneId);
                    if (currentIndex === -1) {
                        this.model.timeZoneId = this.timeZoneOptions[0].key;
                    }
                    
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
        },
        saveChannel: function() {
            console.log(this.model);
            axios.post(`/api/Channels/`, this.model)
                .then(response => {
                    console.log("Saved it!");
                    window.location="/My/Channels";
                    //TODO: Show a success toast or do a redirect;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
        }
    }
});
