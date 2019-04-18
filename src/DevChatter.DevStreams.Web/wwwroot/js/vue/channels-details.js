let app = new Vue({
    el: "#channelDetails",
    data: {
        isLiveComplete: false,
        liveStatus: false,
        channelDataLoaded: false, // TODO: Show a loading message based on this.
        nextStreams: [],
        schedule: [],
        channelId: document.getElementById('channelId').value,
        twitchId: document.getElementById('twitchId').value
    },
    mounted() {
        if (this.twitchId) {
            this.fetchLiveStatus();
        }
        this.fetchChannelData();
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
        fetchChannelData: async function() {
            const response = await axios.post('/graphql',
            {
                query: `query channelQuery($channelId: ID!, $skip: Int!, $take: Int!)
                            { channel(id:$channelId)
                                { id name uri
                                    tags
                                    {
                                        id
                                        name
                                    }
                                    futureStreams (skip: $skip, take: $take)
                                    {
                                        utcStartTime
                                        utcEndTime
                                    }
                                    schedule
                                    {
                                        dayOfWeek
                                        localStartTime
                                        localEndTime
                                        timeZoneId
                                    }
                                }
                            }`,
                variables: {
                    channelId: this.channelId,
                    skip: 0,
                    take: 5
                }
            });

            this.schedule = response.data.data.channel.schedule.map(stream => {
                return {
                    dayOfWeek: moment().isoWeekday(stream.dayOfWeek).format('dddd'),
                    startTime: moment.tz(stream.localStartTime, "HH:mm:ss", stream.timeZoneId).format('h:mm A'),
                    endTime: moment.tz(stream.localEndTime, "HH:mm:ss", stream.timeZoneId).format('h:mm A z')
                };
            });

            this.nextStreams = response.data.data.channel.futureStreams.map(session => {
                return {
                    startTime: moment(session.utcStartTime).calendar(null,{
                        sameElse: 'MMM DD [at] h:mm A'
                    })
                }
            });

            this.channelDataLoaded = true;
        },
        formatTime : function (timeToFormat) {
            return new moment(timeToFormat).calendar();
        }
    }
});
