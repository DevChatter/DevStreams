let app = new Vue({
    el: "#channelDetails",
    data: {
        isLiveCheckComplete: false,
        isLive: false,
        scheduleCheckComplete: false,
        schedule: [],
        twitchId: document.getElementById('twitchId').value
    },
    mounted() {
        this.fetchLiveStatus();
        this.fetchSchedule();
    },
    methods: {
        fetchLiveStatus: function() {
            axios.get(`/api/IsLive/${encodeURIComponent(this.twitchId)}`)
                .then(response => {
                    this.isLive = response.data;
                    this.isLiveCheckComplete = true;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
        },
        fetchSchedule: function() {

        },
    }
});
