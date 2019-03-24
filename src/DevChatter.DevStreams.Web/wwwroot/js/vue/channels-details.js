let app = new Vue({
    el: "#channelDetails",
    data: {
        isLiveComplete: false,
        liveStatus: false,
        scheduleCheckComplete: false,
        schedule: [],
        twitchId: document.getElementById('twitchId').value
    },
    mounted() {
        if (this.twitchId) {
            this.fetchLiveStatus();
        }
        this.fetchSchedule();
    },
    methods: {
        fetchLiveStatus: function() {
            axios.get(`/api/IsLive/${encodeURIComponent(this.twitchId)}`)
                .then(response => {
                    this.liveStatus = response.data === true ? "Live 🔴" : "Offline";
                    this.isLiveComplete = true;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
        },
        fetchSchedule: function() {

        },
    }
});
