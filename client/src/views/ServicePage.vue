<template>
        <div class="main-page">
            <NavBar class="navbar"></NavBar>
            <div class="services-page">
                <SectionList/>
                <WidgetList></WidgetList>
            </div>
        </div>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
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

.main-page {
    font-family: 'inter', sans-serif;
    position: relative;
    z-index: 0;
    height: 100%;
    overflow: hidden;
}

.services-page {
    height: calc(100vh - 5rem);
    outline: none;
    position: relative;
    z-index: -1;
    display: flex;
}

@media (max-width: 920px) {
    .services-page {
        justify-content: center;
        align-items: center;
    }
}

</style>