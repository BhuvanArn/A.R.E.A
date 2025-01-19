<template>
        <div class="main-page">
            <NavBar class="navbar"></NavBar>
            <div v-if="!mobile" class="services-page">
                <SectionList></SectionList>
                <TableAreas></TableAreas>
            </div>
            <div v-if="mobile" class="services-page-mobile">
                <TableAreas></TableAreas>
            </div>
        </div>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import SectionList from '@/components/SectionList.vue';
import TableAreas from '@/components/TableAreas.vue';

export default {
    name: "ServicePage",
    components: {
        SectionList,
        TableAreas,
        NavBar,
    },
    data() {
        return {
            mobile: false,
        };
    },

    mounted() {
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
    },
    methods: {
        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },
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
    background-color: #efefef;
}

.services-page-mobile {
    height: calc(100vh - 5rem);
    outline: none;
    position: relative;
    z-index: -1;
    display: flex;
    background-color: #efefef;
}


@media (max-width: 920px) {
    .services-page {
        justify-content: center;
        align-items: center;
    }
}

</style>