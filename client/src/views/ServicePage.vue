<template>
    <NavBar class="navbar"></NavBar>
    <div class="services-page">
        <SectionList />
    </div>
</template>

<script>
import NavBar from '../components/MainNavBar.vue';
import SectionList from '@/components/SectionList.vue';
import WidgetList from '@/components/WidgetList.vue';

export default {
    name: "ServicePage",
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
.services-page {
    display: flex;
    flex-direction: row;
    font-family: 'inter', sans-serif;
}
</style>