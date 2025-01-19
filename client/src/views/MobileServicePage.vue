<template>
    <div class="main-page">
        <NavBar class="navbar"></NavBar>
        <div class="services-page">
            <SectionList></SectionList>
        </div>
    </div>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import SectionList from '@/components/SectionListMobile.vue';

export default {
name: "ServicePage",
components: {
    SectionList,
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

@media (max-width: 920px) {
.services-page {
    justify-content: center;
    align-items: center;
}
}

</style>