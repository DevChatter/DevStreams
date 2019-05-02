Vue.component('tag-selector', {
    data: function () {
        return {
            selectedTags: [],
            tags: [],
            isLoadingTags: false,
            showMoreTags: false
        };
    },
    props: {
        taggedItems: { type: Array, required: false, default: null },
        showCount: { type: Boolean, required: true }
    },
    template: '#tag-selector-template',
    mounted() {
        this.tagSearch('');
    },
    computed: {
        availableTags: function () {
            return this.tags.filter(tag => this.selectedTags.indexOf(tag) === -1);
        }
    },
    watch: {
        taggedItems: {
            handler: function () {
                this.countTagUsage();
            },
            deep: true
        }
    },

    methods: {
        tagSearch: function (filter) {
            axios.get(`/api/Tags/?filter=${encodeURIComponent(filter)}`)
                .then(response => {
                    this.tags = response.data;
                })
                .catch(error => {
                    console.log(error.statusText);
                });
            this.countTagUsage();
        },
        countTagUsage: function () {
            if (this.showCount) {
                this.tags
                    .forEach(
                        tag => tag.count = this.taggedItems.filter(
                            (x) => x.tags.map(
                                t => t.name).indexOf(tag.name) > -1).length);
                this.tags.sort((a, b) => b.count - a.count);
            }
        },
        showMoreLessTags: function () {
            this.showMoreTags = !this.showMoreTags;
        },
        clickTag: function (tag) {
            var index = this.selectedTags.indexOf(tag);
            if (index > -1) {
                this.selectedTags.splice(index, 1);
            } else {
                this.selectedTags.push(tag);
            }
            this.$emit('selection-changed', this.selectedTags);
        },
        displayTag: function (tag) {
            let text = tag.name;
            if (this.showCount) {
                text += ` (${tag.count})`;
            }
            return text;
        }
    }
});


let app = new Vue({
    el: "#channelList",
    data: {
        isLoadingData: true,
        channels: [],
        searchFilters: {
            selectedTags: []
        }
    },
    mounted() {
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
        tagSelectionChanged: function (tags) {
            this.searchFilters.selectedTags = tags;
        }
    }
});