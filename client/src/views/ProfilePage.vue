<template>
    <body>
        <div class="container">
            <NavBar class="navbar"></NavBar>
            <div class="profile-page">
                <AccountSection></AccountSection>
            </div>
        </div>
    </body>
</template>

<script>
import NavBar from '../components/WorkspaceNavBar.vue';
import AccountSection from '@/components/AccountSection.vue';
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

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
        if (getCookie('token') === null) {
            this.$router.push('/login');
        }
    },
    methods: {
    },

};

</script>

<style>
body {
    height: auto;
    background-color: #efefef;
}

.container {
    display: flex;
    flex-direction: column;
    height: 100vh;
}

.navbar {
    z-index: 10;
    flex: 0 0 auto;
}

.profile-page {
    display: flex;
    flex: 1 1 auto;
    overflow: auto;
    justify-content: center;
    align-items: center;
}

</style>
