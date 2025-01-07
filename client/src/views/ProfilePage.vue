<template>
    <body>
        <NavBar class="navbar"></NavBar>
        <div class="profile-page">
<!--             <div class="navigation-container">

            </div> -->
            <AccountSection></AccountSection>
        </div>
    </body>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import AccountSection from '@/components/AccountSection.vue';

export default {
    name: "ProfilePage",
    components: {
        NavBar,
        AccountSection,
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

<style>
body {
    background-color: #efefef;
}

.navbar {
    z-index: 10;
}

.profile-page {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    width: 100%;
    display: flex;
    flex-direction: column;
    font-family: 'inter', sans-serif;
    z-index: 1;
}

.navigation-container {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 15rem;
    min-height: 15rem;
    background-color: transparent;
}
</style>