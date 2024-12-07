<template>
    <NavBar></NavBar>

    <body>
        <div>
            <ServicePage />
        </div>
    </body>
</template>

<script>
import NavBar from '../components/MainNavBar.vue';
import ServicePage from '@/components/ServicePage.vue';

export default {
    name: 'AreaHome',
    data() {
        return {
            // data
        }
    },
    components: {
        NavBar,
        ServicePage,
    },
    methods: {
        // methods
    }
}
</script>

<style></style>