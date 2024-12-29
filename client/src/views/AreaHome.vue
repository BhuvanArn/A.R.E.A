<template>
    <NavBar class="navbar"></NavBar>
    <body>
        <div class="page-container">
            <div class="top-container">
                <img class="img-01-style" src="@/assets/icone-puzzle.png">
                <h1 class="title-home-page">Action REAction</h1>
            </div>
            <div class="middle-container">
                <div class="filler01-homepage"></div>
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/service.png" class="img-02-style">
                        <h3 class="title-dialog-txt">Several services available</h3>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt">Our platform offers a wide range of services, to help you automate your daily tasks.</p>
                    </div>
                </div>
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/custom.png" class="img-02-style">
                        <h3 class="title-dialog-txt">Customization</h3>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt">Thanks to the Action and REAction components, available for each service, customize your automation as you wish.</p>
                    </div>
                </div>
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/perf.png" class="img-02-style">
                        <h3 class="title-dialog-txt">Strong performances</h3>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt">Our platform is designed to offer you the best performance possible.</p>
                    </div>
                </div>
                <div class="filler01-homepage"></div>
            </div>
            <div class="bottom-container">

            </div>
        </div>
    </body>
</template>

<script>
import NavBar from '../components/MainNavBar.vue';

export default {
    name: 'AreaHome',
    data() {
        return {
            // data
        }
    },
    components: {
        NavBar,
    },
    methods: {
        // methods
    },
    onMounted() {
        const expirationTime = localStorage.getItem('expirationTime');
        if (token && expirationTime && currentTime >= expirationTime) {
            localStorage.setItem('Status', 'LoggedOut');
        }
    }
}
</script>

<style>

body {
    background-color: #efefef;
}

.page-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: auto;
    margin: 0;
    padding: 0;
}

.img-01-style {
    width: 30rem;
    height: 30rem;
}

.top-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    margin-top: 5rem;
    margin-bottom: 5rem;
}

.title-home-page {
    font-size: 3rem;
    font-family: 'inter', sans-serif;
    color: #969696;
    margin-left: 1rem;
    font-weight: 700;
    font-size: 7rem;
}

.middle-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: 20rem;
    background-color: #bcc1ba;
}

.dialog-container {
    display: flex;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: 100%;
    height: 10rem;
    background-color: transparent;
    margin-left: 1rem;
    margin-right: 1rem;
}

.title-dialog {
    display: flex;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: 5rem;
    background-color: transparent;
    margin-left: 1rem;
    margin-right: 1rem;
}

.title-dialog-txt {
    font-size: 2rem;
    font-family: 'inter', sans-serif;
    margin-left: 1rem;
    font-weight: 500;
    color: #353535;
}

.desc-dialog-txt {
    font-size: 1.5rem;
    font-family: 'inter', sans-serif;
    margin-left: 1rem;
    margin-right: 1rem;
    font-weight: 500;
    color: #353535;
}

.img-02-style {
    width: 4rem;
    height: 4rem;
    background-color: transparent;
}

.filler01-homepage {
    width: 15rem;
    height: 100%;
    background-color: transparent;
}

.dialog-desc {
    display: flex;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: 8rem;
    background-color: transparent;
    padding-top: 0.5rem;
    background-color: transparent;
}

.bottom-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: 20rem;
    background-color: #efefef;
}

</style>
