Vue.component('vue-multiselect', window.VueMultiselect.default);

let app = new Vue({
    components: {
        Multiselect: window.VueMultiselect.default
    },
    el: "#channelList",
    data: {
        isLoadingData: true,
        channels: [],
        searchFilters: {
            selectedTags: [],
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