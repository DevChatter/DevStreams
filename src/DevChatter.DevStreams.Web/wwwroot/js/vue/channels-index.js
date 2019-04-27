let app = new Vue({
    el: "#channelList",
    data: {
        isLoadingData: true,
        channels: [],
        searchFilters: {
            selectedTags: []
        },
        tags: [],
        isLoadingTags: false,
        showMoreTags: false
    },
    mounted() {
        this.tagSearch('');
        this.fetchChannels();
    },
    watch:{
        searchFilters: {
            handler: async function () {
                await this.fetchChannels();
            },
            deep: true
          }
    },
    computed: {
        availableTags: function() {
            const selectedTags = this.searchFilters.selectedTags;
            return this.tags
                .filter(tag => tag.count > 0 && selectedTags.indexOf(tag) === -1)
                .sort((a,b) => b.count - a.count);
        }
    },
    methods: {
        fetchChannels: async function () {
            const res = await axios.post('/graphql',
                {
                    query: `query channelQuery($tagIds: [ID]!)
                            { channels(tagIds:$tagIds)
                                { id name uri
                                    tags
                                    {
                                        id
                                        name
                                    }
                                    nextStream
                                    {
                                        utcStartTime
                                        utcEndTime
                                    }
                                }
                            }`,
                    variables: {
                        tagIds: this.searchFilters.selectedTags.map(tag => tag.id)
                    }
                });
            this.channels = res.data.data.channels;
            if (this.channels) {
                this.tags
                    .forEach(
                        tag => tag.count = this.channels.filter(
                            (x) =>  x.tags.map(
                                t => t.name).indexOf(tag.name) > -1).length);
            }
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
                return new moment(streamSession.utcStartTime).calendar();
            }
            return "None Scheduled";
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
        showMoreLessTags: function() {
            this.showMoreTags = !this.showMoreTags;
        },
        clickTag: function(tag) {
            var index = this.searchFilters.selectedTags.indexOf(tag);
            if (index > -1) {
                this.searchFilters.selectedTags.splice(index, 1);
            } else {
                this.searchFilters.selectedTags.push(tag);
            }
        }
    }
});