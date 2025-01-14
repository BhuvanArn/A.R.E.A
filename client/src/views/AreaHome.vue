<template>
    <NavBar class="navbar"></NavBar>
    <body>
        <div class="page-container">
            <div class="top-container" :class="{ 'top-container-mobile': mobile }">
                <img class="img-01-style" src="@/assets/icone-puzzle.png" alt="icone-puzzle">
                <h1 class="title-home-page" :class="{ 'title-home-page-mobile': mobile }">Action REAction</h1>
            </div>
            <div class="middle-container" :class="{ 'middle-container-mobile': mobile }">
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/service.png" class="img-02-style" alt="service">
                        <h2 class="title-dialog-txt" :class="{ 'title-dialog-txt-tablet': tablet }">Several services available</h2>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt" :class="{ 'desc-dialog-txt-tablet': tablet }">Our platform offers a wide range of services, to help you automate your daily tasks.</p>
                    </div>
                </div>
                <div v-if="!mobile" class="filler02-homepage"></div>
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/custom.png" class="img-02-style" alt="custom">
                        <h2 class="title-dialog-txt" :class="{ 'title-dialog-txt-tablet': tablet }">Customization</h2>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt" :class="{ 'desc-dialog-txt-tablet': tablet }">Thanks to the Action and REAction components, available for each service, customize your automation as you wish.</p>
                    </div>
                </div>
                <div v-if="!mobile" class="filler02-homepage"></div>
                <div class="dialog-container">
                    <div class="title-dialog">
                        <img src="@/assets/perf.png" class="img-02-style" alt="perf">
                        <h2 class="title-dialog-txt" :class="{ 'title-dialog-txt-tablet': tablet }">Strong performances</h2>
                    </div>
                    <div class="dialog-desc">
                        <p class="desc-dialog-txt" :class="{ 'desc-dialog-txt-tablet': tablet }">Our platform is designed to offer you the best performance possible.</p>
                    </div>
                </div>
            </div>
            <div class="middle-02-container">
                <div class="middle-02-container-content">
                    <h2 class="middle-02-title" :class="{ 'middle-02-title-mobile': mobile }">Build YOUR powerful workflows</h2>
                    <p class="middle-02-content" :class="{ 'middle-02-content-mobile': mobile }">Area puts the power of automation in your hands => no coding required! Take your workflows to the next level with our suite of automation tools.</p>
                </div>
                <div class="middle-02-container-img">
                    <img src="@/assets/home-img-01.png" class="img-03-style" :class="{ 'img-03-style-mobile': mobile }" alt="Example of a workflow">
                </div>
            </div>
            <div class="middle-03-container">
                <div class="filler04"></div>
                <h2 class="middle-03-title" :class="{ 'middle-03-title-mobile': mobile }">Get started right now with our list of services</h2>
                <p class="middle-03-content" :class="{ 'middle-03-content-mobile': mobile }">Thanks to our platform, you can automate your daily tasks in a few clicks. Discover our list of services and start automating your tasks now!</p>
                <div class="sample-services-list-container" :class="{ 'sample-services-list-container-mobile': mobile }">
                    <img src="@/assets/oauth/google_logo.png" class="img-04-style" alt="Google logo">
                    <img src="@/assets/oauth/discord_logo.png" class="img-04-style" alt="Discord logo">
                    <img src="@/assets/oauth/github_logo.png" class="img-04-style" alt="GitHub logo">
                    <img src="@/assets/oauth/dropbox_logo.png" class="img-04-style" alt="Dropbox logo">
                    <img src="@/assets/oauth/spotify_logo.png" class="img-04-style" alt="Spotify logo">
                </div>
                <div class="filler04"></div>
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
            mobile: false,
            tablet: false,
        }
    },

    components: {
        NavBar,
    },

    methods: {
        checkScreen() {
            this.windowWidth = window.innerWidth;

            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }

            if (this.windowWidth <= 1500) {
                this.tablet = true;
            } else {
                this.tablet = false;
            }
        },
    },

    onMounted() {
        const expirationTime = localStorage.getItem('expirationTime');
        if (token && expirationTime && currentTime >= expirationTime) {
            localStorage.setItem('Status', 'LoggedOut');
        }
    },

    mounted() {
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
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
    margin-top: 10rem;
    margin-bottom: 5rem;
}

.title-home-page {
    font-size: 3rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin-left: 1rem;
    font-weight: 700;
    font-size: 7rem;
}

.middle-container {
    display: flex;
    justify-content: center;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: auto;
    min-height: 17rem;
    background-color: #bcc1ba;
}

.dialog-container {
    display: flex;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: auto;
    height: auto;
    margin-left: 4rem;
    margin-right: 4rem;
    margin-top: 2rem;
    margin-bottom: 2rem;
    word-wrap: break-word;
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
    font-weight: 500;
    color: #353535;
    width: auto;
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

.filler03-homepage {
    width: 10rem;
    height: 80%;
    background-color: transparent;
}

.dialog-desc {
    display: flex;
    flex-direction: row;
    background-color: transparent;
    width: 100%;
    height: auto;
    max-width: 60rem;
    padding-top: 1rem;
    text-align: start;
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

.top-container-mobile {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: 100%;
    margin-top: 10rem;
    margin-bottom: 5rem;
}

.title-home-page-mobile {
    font-size: 3rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin-left: 1rem;
    font-weight: 700;
    font-size: 5rem;
    text-align: center;
}

.middle-container-mobile {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: 100%;
    height: auto;
    background-color: #bcc1ba;
}

.filler02-homepage {
    width: 5rem;
    height: 100%;
    background-color: transparent;
}

.title-dialog-txt-tablet {
    font-size: 1.3rem;
    font-family: 'inter', sans-serif;
    margin-left: 1rem;
    font-weight: 500;
    color: #353535;
}

.desc-dialog-txt-tablet {
    font-size: 1rem;
    font-family: 'inter', sans-serif;
    margin-left: 1rem;
    font-weight: 500;
    color: #353535;
    width: auto;
    height: auto;
    flex-wrap: wrap;
    max-width: 30rem;
}

.navbar {
    position: fixed;
    z-index: 100;
}

.middle-02-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: 100%;
    height: auto;
    background-color: #efefef;
    margin-top: 5rem;
    margin-bottom: 5rem;
}

.middle-02-container-content {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    background-color: transparent;
    width: 100%;
    height: auto;
    text-align: center;
    margin-top: 2rem;
    margin-bottom: 3.5rem;
}

.middle-02-title {
    font-size: 3rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 700;
    margin-bottom: 1rem;
}

.middle-03-title {
    font-size: 3rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 700;
    margin-bottom: 1rem;
    width: 50rem;
    text-align: center;
}

.middle-02-content, .middle-03-content {
    font-size: 1.5rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 500;
    width: 50%;
    text-align: center;
}

.middle-02-title-mobile {
    font-size: 2rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 700;
    margin-bottom: 1rem;
}

.middle-03-title-mobile {
    font-size: 2rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 700;
    margin-bottom: 1rem;
    width: 25rem;
    text-align: center;
}

.middle-02-content-mobile, .middle-03-content-mobile {
    font-size: 1.2rem;
    font-family: 'inter', sans-serif;
    color: #353535;
    margin: 0;
    padding: 0;
    font-weight: 500;
    width: 80%;
    text-align: center;
}

.middle-02-container-img {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: auto;
    height: auto;
    background-color: #efefef;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.4);
}

.img-03-style {
    width: 60rem;
    height: 32rem;
}

.img-03-style-mobile {
    width: 25rem;
    height: 14rem;
}

.middle-03-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    width: 100%;
    height: auto;
    background-color: #9fb7b9;
    margin-top: 3rem;
    margin-bottom: 3rem;
    min-height: 20rem;
}

.sample-services-list-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    background-color: transparent;
    width: auto;
    height: auto;
    margin-top: 2rem;
    background-color: transparent;
    gap: 2rem;
    overflow: hidden;
}

.sample-services-list-container-mobile {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: row;
    flex-wrap: wrap;
    background-color: transparent;
    width: 20rem;
    height: auto;
    margin-top: 2rem;
    gap: 1rem;
}

.sample-services-list-container-mobile .img-04-style {
    width: calc(25% - 1rem);
    height: auto;
}

.img-04-style {
    width: 5rem;
    height: 5rem;
    margin: 1rem;
    transition: transform 0.3s ease-in-out;
    background-color: transparent;
}

.img-04-style:hover {
    cursor: pointer;
    transform: scale(1.2);
}

.filler04 {
    width: 100%;
    height: 2rem;
    background-color: transparent;
}

</style>
