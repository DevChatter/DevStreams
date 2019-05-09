<template>
    <div>
        <label class="control-label" for="ajax">Selected Tags</label>
        <div class="tag-selection">
            <span v-for="(tag, index) in selectedTags" class="tag-selected btn-success">
                <span>{{ tag.name }}</span>
                <span class="tag-remove-icon" v-on:click="clickTag(tag)"></span>
            </span>
            <span v-if="selectedTags.length == 0">None Selected</span>
        </div>
        <label class="control-label">Available Tags</label>
        <div class="tag-selection">
            <span v-for="(tag, index) in availableTags" v-if="showMoreTags || index < 5" class="tag-available">
                <span v-on:click="clickTag(tag)">{{tag.name}} ({{tag.count}})</span>
            </span>
        </div>
        <button v-on:click="showMoreLessTags"
            v-if="availableTags.length > 5">{{showMoreTags ? 'Show less...' : 'Show more...'}}</button>
    </div>
</template>

<script>
module.exports = {
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
            return this.tags.filter(tag => this.selectedTags.indexOf(tag) === -1
                && tag.count < this.taggedItems.length
                && tag.count > 0);
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
};
</script>