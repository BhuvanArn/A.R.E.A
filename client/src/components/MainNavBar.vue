<template>
    <header :class="{'scrolled-nav': scrollPosition}">
      <nav>
        <div class="navbar-container" :class="{ navbarMobileActive: mobileNav }">
          <hr class="vertical-hr">
          <img src="@/assets/logo.png" class="logo">
          <div class="mb-menu-selector-style" :class="{ mbMenuSelectorStyleActive: mobileNav }">
          </div>
        </div>
        <ul v-show="!mobile" class="navigation">
<!--           <li><router-link class="link" :to="{name: 'home'}" replace><button @click="navigateToHome" class="navbar-button">Home</button></router-link></li>
          <li><router-link class="link" :to="{name: ''}"><button class="navbar-button">About</button></router-link></li>
          <li><router-link class="link" :to="{name: 'plans'}"><button @click="navigateToPlans" class="navbar-button">Plans</button></router-link></li>
          <li><router-link class="link" :to="{name: 'access'}" replace><button @click="navigateToAccess" class="navbar-button">Access</button></router-link></li> -->
        </ul>
        <transition name="mobile-nav" :class="{ mobileNavActive: mobileNav }">
            <ul v-show="mobileNav" class="mobile-menu" :class="{ mobileMenuActive: mobileNav }">
<!--            <li v-show="mobileNavButton"><router-link class="link" :to="{name: 'home'}"><button @click="navigateToHome" class="mobile-navbar-button">Home</button></router-link></li>
                <li v-show="mobileNavButton"><router-link class="link" :to="{name: ''}"><button class="mobile-navbar-button">About</button></router-link></li>
                <li v-show="mobileNavButton"><router-link class="link" :to="{name: 'plans'}"><button @click="navigateToPlans" class="mobile-navbar-button">Plans</button></router-link></li>
                <li v-show="mobileNavButton"><router-link class="link" :to="{name: 'access'}"><button @click="navigateToAccess" class="mobile-navbar-button">Access</button></router-link></li> -->
            </ul>
        </transition>
      </nav>
    </header>
  </template>
  
<script>
export default {
  name: "navigation",
  data() {
    return {
      scrollPosition: false,
      mobile: false,
      mobileNav: false,
      windowWidth: false,
      isRotated: false,
      mobileNavButton: false,
    }
  },

  mounted() {
    window.addEventListener('resize', this.checkScreen);
    window.addEventListener('resize', this.enforceMinWidth);
    this.checkScreen();
    this.enforceMinWidth();
  },

  methods: {
    toggleMobileNav() {
      if (!this.isRotated) {
        this.isRotated = true;
        this.mobile = true;
        this.mobileNav = true
      } else {
        this.isRotated = false;
        this.mobileNav = false;
      }
      if (this.mobileNav) {
        setTimeout(() => {
          if (this.mobileNav) {
            this.mobileNavButton = true;
          }
        }, 350);
      } else {
        setTimeout(() => {
          this.mobileNavButton = false;
        }, 0);
      }
    },

    checkScreen() {
      this.windowWidth = window.innerWidth;

      if (this.windowWidth <= 1020) {
        this.mobile = true;
      } else {
        this.mobile = false;
        this.mobileNav = false;
        this.isRotated = false;
        this.mobileNavButtone = false;
      }
    },

    enforceMinWidth() {
      this.windowWidth = window.innerWidth;

      if (this.windowWidth <= 300) {
        window.resizeTo(300, window.innerHeight);
      }
    },

    navigateToHome(event) {
      event.preventDefault();
/*         window.location.href = this.$router.resolve({ name: 'home' }).href; */
    },

    navigateToAccess(event) {
      event.preventDefault();
/*         window.location.href = this.$router.resolve({ name: 'access' }).href; */
    },

    navigateToPlans(event) {
      event.preventDefault()
/*         window.location.href = this.$router.resolve({ name: 'plans' }).href; */
    }

  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

header {
  background-color: #bcc1ba;
  z-index: 99;
  width: 100%;
  transition: .5s ease all;
  color: white;
  border-bottom: 1px solid #000000;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);

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

    ul,
    .link {
      font-weight: 500;
      color: white;
      list-style-type: none;
      text-decoration: none;
      transition: .8s ease all;
    }
  }
}

li {
  text-transform: uppercase;
  padding: 16px;
  margin-left: 16px;
  list-style-type: none;
  transition: .5s ease all;
}

.navbar-button{
    width: 8rem;
    height: 3rem;
    border: transparent;
    background-color: transparent;
    transition: box-shadow 0.35s ease;
    border-radius: 1rem;
  font-family: 'bold', sans-serif;
  color: rgba(14, 91, 199, 0.7);
  font-size: large;
}

.navbar-button:hover{
    box-shadow: 12px 12px 12px rgba(0, 0, 0, 0.1),
    -10px -10px 10px white;
  cursor: pointer;
}

.navbar-button:active{
    box-shadow: 12px 12px 12px rgba(0, 0, 0, 0.1) inset,
    -10px -10px 10px white inset;
}

.mb-nav-button {
  display: flex;
  cursor: pointer;
  align-items: center;
  position: absolute;
  width: 30px;
  height: 30px;
  right: 24px;
  top: 1.5rem;
  transition: .8s ease all;
}

.rotated {
  transform: rotate(180deg);
}

.logo {
  display: flex;
  width: 5.5rem;
  height: 6rem;
  align-items: center;
  left: 24px;
  top: 0;
  padding-top: 1rem;
  padding-bottom: 1rem;
  padding-left: 1rem;
  transition: .5s ease all;
}

.navigation {
  display: flex;
  position: absolute;
  align-items: center;
  justify-content: center;
  width: 100%;
  top: -1rem;
  left: -3rem;
  border-radius: 20px;
}

.link {
  font-size: 14px;
  transition: .5 ease all;
  padding-bottom: 4px;
  border-bottom: 1px solid transparent;
  transition: 2s ease all;
}

.navbar-container {
  display: flex;
  height: 5rem;
  align-items: center;
  flex-direction: row;
  transition: .5s ease all;
}

.navbarMobileActive {
  height: 20rem;
}

.mb-menu-selector-style{
  background-color: rgba(19, 107, 230, 0.7);
  position: absolute;
  width: 0;
  height: 0;
  top: 4rem;
  left: 5rem;
  transition: .2s ease all;
}

.mbMenuSelectorStyleActive{
  background-color: rgba(19, 107, 230, 0.7);
  position: absolute;
  width: 0.2rem;
  height: 12rem;
  top: 4rem;
  left: 5rem;
  transition: .5s ease all;
}

.mobileMenuActive {
  display: flex;
  position: absolute;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 6rem;
  right: 0;
  bottom: 0;
  flex-direction: column;
  height: 100%;
  width: 10rem;
  margin: 0;
  padding: 0;
  transition: 2s ease all;
}

.mobile-nav {
  transition: 0.5s ease all;
}

.mobileNavActive {
  transition: 0.5s ease all;
}

.mobile-menu {
  display: flex;
  position: absolute;
  align-items: center;
  justify-content: center;
  top: 0;
  left: 6rem;
  right: 0;
  bottom: 0;
  flex-direction: column;
  height: 100%;
  width: 10rem;
  margin: 0;
  padding: 0;
  transition: 0s ease all;
}

.mobile-navbar-button {
  width: 8rem;
    height: 3rem;
    border: wheat;
    background-color: rgb(255, 255, 255);
    border-radius: 1rem;
  cursor: pointer;
  margin: -0.5rem;
  font-family: 'bold', sans-serif;
  color: rgba(14, 91, 199, 0.7);
  font-size:larger;
}
  
.mobile-navbar-button:active{
  box-shadow: 12px 12px 12px rgba(0, 0, 0, 0.1) inset,
  -10px -10px 10px white inset;
}

.vertical-hr {
  width: 1px;
  height: 100%;
  background-color: black;
  border: none;
  margin: 0 10px;
}

</style>
  