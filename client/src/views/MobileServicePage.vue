<template>
    <NavBar class="navbar"></NavBar>
    <div class="services-page">
        <SectionList/>
    </div>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import SectionList from '@/components/SectionListMobile.vue';

export default {
    name: "MobileServicePage",
    components: {
        SectionList,
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