<template>
    <div class="services-page">
        <SectionList :sections="sectionNames" />
        <WidgetList />
    </div>
</template>

<script>
import SectionList from './SectionList.vue';
import WidgetList from './WidgetList.vue';

export default {
    name: "ServicePage",
    components: {
        SectionList,
        WidgetList,
    },
    data() {
        return {
            sectionNames: [],
        };
    },

    mounted() {
        this.fetchSections();
    },
    methods: {
        fetchSections() {
            // TODO Fetch services from server here
            this.sectionNames = ["Google", "Gmail", "Discord", "Timer", "Outlook"];
        },
    },

};
</script>

<style scoped>
.services-page {
    display: flex;
    flex-direction: row;
}
</style>