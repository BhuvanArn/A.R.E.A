<template>
    <NavBar class="navbar"></NavBar>
    <div class="services-page">
        <SectionList/>
    </div>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import SectionList from '@/components/SectionListMobile.vue';
import WidgetList from '@/components/WidgetList.vue';

export default {
    name: "MobileServicePage",
    components: {
        SectionList,
        WidgetList,
        NavBar,
    },
    data() {
        return {
        };
    },

    mounted() {
    },
    methods: {
    },

};
</script>

<style scoped>

.navbar {
    position: relative;
    z-index: 10;
}

.services-page {
    position: relative;
    display: flex;
    flex-direction: row;
    font-family: 'inter', sans-serif;
    z-index: 1;
    height: 100vh;
    width: auto;
}

</style>