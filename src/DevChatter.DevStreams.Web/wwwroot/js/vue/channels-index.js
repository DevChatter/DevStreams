let app = new Vue({
    el: "#channelList",
    data: {
        isLoadingData: true,
        channels: [],
        searchFilter: ''
    },
    mounted() {
        this.fetchChannels();
    },
    methods: {
        fetchChannels: async function () {
            const res = await axios.post('/graphql',
                {
                    query: `query getChannels
                        { channels
                            { id name
                                tags
                                {
                                    name
                                }
                                nextStream
                                {
                                    utcStartTime 
                                    utcEndTime
                                }
                            }
                        }`
                });
            this.channels = res.data.data.channels;
            this.isLoadingData = false;
        },
        formatTags: function (tags) {
            if (tags) {
                return tags.map(tag => tag.name).join(', ');
            }
            return "-";
        },
        formatStartTime: function (streamSession) {
            if (streamSession) {
                // TODO: Show in correct time zone.
                return streamSession.utcStartTime;
            }
            return "None Scheduled";
        }
    }
});