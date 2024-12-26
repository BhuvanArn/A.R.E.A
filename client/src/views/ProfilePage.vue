<template>
    <body>
        <NavBar class="navbar"></NavBar>
        <div class="profile-page">

        </div>
    </body>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';

export default {
    name: "ProfilePage",
    components: {
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

<style>
body {
    background-color: #efefef;
}

.navbar {
    position: fixed;
    z-index: 10;
}

.profile-page {
    position: relative;
    height: 100vh;
    width: 100%;
    display: flex;
    flex-direction: row;
    font-family: 'inter', sans-serif;
    z-index: 1;
}
</style>