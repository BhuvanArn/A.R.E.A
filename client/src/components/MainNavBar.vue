<template>
  <header>
    <div class="navbar-container" :class="{ 'navbar-container-mobile-active': mobileNav }">
      <img @click.stop="displayMenu" src="@/assets/menu.png" class="menu" :class="{ rotated: isRotated }">
      <hr class="vertical-hr" :class="{ 'vertical-hr-mobile': mobile }">
      <img src="@/assets/logo.png" class="logo">
      <h2 class="title">Area</h2>
      <div class="filler01">
      </div>
      <div v-show="!isLogged && !mobile" class="nv-btn-container">
        <router-link><button @click="navigateToLogin" class="access-btn-style">Login</button></router-link>
        <router-link><button @click="navigateToRegister" class="access-btn-style">Register</button></router-link>
      </div>
      <hr v-show="!mobile" class="vertical-hr">
    </div>
    <div v-show="mobileNav" class="mobile-menu-container">
      <button>Sample</button> <!-- To replace with the correct links/buttons -->
      <button>Sample</button> <!-- To replace with the correct links/buttons -->
      <button>Sample</button> <!-- To replace with the correct links/buttons -->
      <button>Sample</button> <!-- To replace with the correct links/buttons -->
      <hr class="horizontal-hr">
      <div v-show="!isLogged" class="nv-btn-container-mobile">
        <router-link><button @click="navigateToLogin" class="access-btn-style">Login</button></router-link>
        <router-link><button @click="navigateToRegister" class="access-btn-style">Register</button></router-link>
      </div>
    </div>
    <div :class="{ activeMenu : isActiveMenu }" ref="isActiveMenu">
    </div>
  </header>
</template>

<script>

export default {
  name: "navigation",
  data() {
    return {
      mobile: false,
      mobileNav: false,
      windowWidth: false,
      isRotated: false,
      mobileNavButton: false,
      isLogged: false,
      isActiveMenu: false,
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

    toggleMenu() {
      if (!this.isActiveMenu) {
        document.addEventListener('click', (event) => this.handleClickOutside(event, this.$refs.isActiveMenu));
        this.isRotated = true;
        this.isActiveMenu = true;
      } else {
        this.isRotated = false;
        this.isActiveMenu = false;
        document.removeEventListener('click', (event) => this.handleClickOutside(event, this.$refs.isActiveMenu));
      }
    },

    handleClickOutside(event, menuRef) {
      if (this.mobileNav)
        return;
      if (menuRef && !menuRef.contains(event.target)) {
        if (menuRef === this.$refs.isActiveMenu) {
          this.isActiveMenu = false;
          this.isRotated = false;
        }
        document.removeEventListener('click', (event) => this.handleClickOutside(event, menuRef));
      }
    },

    checkScreen() {
      this.windowWidth = window.innerWidth;

      if (this.windowWidth <= 960) {
        this.mobile = true;
      } else {
        this.mobile = false;
        this.mobileNav = false;
        this.isRotated = false;
        this.mobileNavButtone = false;
        this.isActiveMenu = false;
      }
    },

    enforceMinWidth() {
      this.windowWidth = window.innerWidth;

      if (this.windowWidth <= 300) {
        window.resizeTo(300, window.innerHeight);
      }
    },

    navigateToLogin(event) {
      event.preventDefault()
      window.location.href = this.$router.resolve({ name: 'login' }).href;
    },

    navigateToRegister(event) {
      event.preventDefault()
      window.location.href = this.$router.resolve({ name: 'register' }).href;
    },

    displayMenu() {
      if (this.mobile) {
        this.toggleMobileNav();
      } else {
        this.toggleMenu();
      }
    },
  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.rotated {
  transform: rotate(180deg);
}

header {
  z-index: 99;
  width: 100%;
  color: white;
  background-color: #bcc1ba;

  nav {
    display: flex;
    position: relative;
    flex-direction: row;
    padding: 12px 0;
    width: 90%;
    margin: 0;
    padding: 0;
    box-sizing: border-box;

    @media(min-width: 1080px) {
      max-width: 1080px;
    }

    ul,
    .link {
      font-weight: 500;
      color: white;
      list-style-type: none;
      text-decoration: none;
    }
  }
}

li {
  text-transform: uppercase;
  padding: 16px;
  margin-left: 16px;
  list-style-type: none;
}

.navbar-button{
    width: 8rem;
    height: 3rem;
    border: transparent;
    background-color: transparent;
    transition: box-shadow 0.35s ease;
    border-radius: 1rem;
  font-family: 'inter', sans-serif;
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
  width: 5rem;
  height: 6.2rem;
  align-items: center;
  left: 24px;
  top: 0;
  padding-top: 1rem;
  padding-bottom: 1rem;
  transition: .5s ease all;
}

.menu {
  width: 3rem;
  height: 3rem;
  margin-left: 1rem;
  margin-right: 0.5rem;
  transition: .5s ease all;
  cursor: pointer;
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
  border-bottom: 1px solid #000000;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
  background-color: #bcc1ba;
  color: white;
}

.navbar-container-mobile-active {
  display: flex;
  height: 5rem;
  align-items: center;
  flex-direction: row;
  border-bottom: 1px solid #000000;
  box-shadow: none;
}

.mobile-menu-container {
  display: flex;
  height: 15rem;
  align-items: center;
  flex-direction: column;
  border-bottom: 1px solid #000000;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
  animation:backwards 1s;
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

.vertical-hr {
  width: 1px;
  height: 100%;
  background-color: black;
  border: none;
  margin: 0 10px;
}

.horizontal-hr {
  width: 100%;
  height: 1px;
  background-color: black;
  border: none;
  margin: 0 10px;
}

.vertical-hr-mobile {
  width: 2px;
}

.filler01 {
  width: 90%;
  height: 100%;
  background-color: transparent;
  border: transparent;
}

.nv-btn-container {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  width: 20rem;
  height: 100%;
  background-color: transparent;
  gap: 1rem;
}

.nv-btn-container-mobile {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  width: 20rem;
  height: 25%;
  background-color: transparent;
  gap: 1rem;
  margin: 1rem;
}

.access-btn-style {
  width: 8rem;
  height: 3rem;
  background-color: transparent;
  border: 1px solid rgba(0, 0, 0, 0.7);
  transition: box-shadow 0.35s ease;
  border-radius: 10px;
  font-family: 'inter', sans-serif;
  color: rgba(0, 0, 0, 0.7);
  font-size: large;
}

.access-btn-style:hover {
  background-color: #abaeaa;
  cursor: pointer;
}

.access-btn-style:active {
  background-color: #c6c9c4;
}

.title {
  font-size: 3rem;
  font-weight: 400;
  font-family: 'inter', sans-serif;
  color: rgba(0, 0, 0, 0.7);
  margin-left: 1rem;
  margin-right: 1rem;
}

.activeMenu {
  position: absolute;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 15rem;
  height: 20rem;
  background-color: #bcc1ba;
  border: 1px solid black;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
}
</style>
  