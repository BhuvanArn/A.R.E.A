<template>
    <header>
        <nav>
            <div v-show="sideMenu" class="side-menu">
                <ul class="side-menu-list">
                    <li class="li-style" @click="goTo('/')">Page 1</li>
                    <hr class="hr-style">
                    <li class="li-style" @click="goTo('/response-table')">Page 2</li>
                    <hr class="hr-style">
                    <li class="li-style" @click="goTo('/')">Page 3</li>
                    <hr class="hr-style">
                    <li class="li-style" @click="goTo('/')">Page 4</li>
                </ul>
            </div>
            <div class="navbar-container">
                <img @click="toggleSideMenu" src="@/assets/menu.png" class="menu-img">
                <h1 class="title">AREA TEST</h1>
                <span class="profile-img-container">
                    <img src="@/assets/profile.png" class="profile-img">
                </span>
            </div>
        </nav>
    </header>
</template>

<script>
export default {
    name: 'NavBar',
    data() {
        return {
            sideMenu: false
        }
    },
    methods: {
        toggleSideMenu() {
            this.sideMenu = !this.sideMenu;
        },

        goTo(route) {
            this.$router.push(route);
        }
    }
};
</script>

<style>

header {
    background-color: #f9f9f9;
    z-index: 99;
    width: 99%;
    transition: .5s ease all;
    color: white;
    border-radius: 20px;
    top: 1rem;
    left: 0.5%;

    nav {
        display: flex;
        position: relative;
        flex-direction: row;
        padding: 12px 0;
        transition: .5s ease all;
        width: 90%;
        margin: auto;

        @media(min-width: 1080px) {
            max-width: 1080px;
        }
    }
}

.navbar-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    width: 100%;
    height: 5rem;
    margin: auto;
    padding: 0 20px;
    background-color: rgb(244, 241, 236);
}

.profile-img {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    right: 0;
}

.profile-img-container:hover {
    cursor: pointer;
}

.profile-img-container{
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 3.5rem;
    height: 3.5rem;
    border-radius: 50%;
    overflow: hidden;
    right: 0;
    margin: 1rem;
}

.menu-img {
    width: 30px;
    height: 30px;
    margin-right: 1rem;
}

.menu-img:hover {
    cursor: pointer;
}

.title {
    color: black;
    font-size: 2.5rem;
    font-family: Arial, Helvetica, sans-serif;
}

.side-menu {
    position: absolute;
    background-color: #e5e4e4;
    border: transparent;
    border-radius: 20px;
    width: 10rem;
    height: 12rem;
    left: 0;
    top: 5rem;
    transition: .5s ease all;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}

.side-menu-list {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
    padding: 0;
    margin: 0;
    list-style: none;
}

.hr-style {
    width: 90%;
}

.li-style {
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 1.5rem;
    font-family: Arial, Helvetica, sans-serif;
    color: black;
    width: 100%;
}

.li-style:hover {
    cursor: pointer;
    background-color: #6c6a6a;
}

</style>