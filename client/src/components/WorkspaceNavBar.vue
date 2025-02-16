<template>
  <div class="navbar-container" :class="{ 'navbar-container-mobile-active': mobileNav }">
    <nav class="header">
      <img v-if="mobile" @click.stop="displayMenu" src="@/assets/menu.png" class="menu" :class="{ rotated: isRotated }" alt="Menu">
      <hr class="vertical-hr" :class="{ 'vertical-hr-mobile': mobile }">
      <img src="@/assets/logo.png" class="logo" alt="Logo" @click="navigateToHome">
      <h2 class="title" @click="navigateToHome">Area</h2>
      <div class="filler01">
      </div>
      <hr v-show="!mobile" class="vertical-hr">
      <div class="user-info-container" v-show="!mobile">
          <h2 class="username-txt" @click="goToProfilePage">{{ userName }}</h2>
          <span @click="goToProfilePage" class="user-avatar-img-container">
            <h2 class="user-avatar-ini2">{{ userAvatar }}</h2>
          </span>
      </div>
    </nav>
  </div>
  <div v-show="mobileNav" class="mobile-menu-container">
    <div class="user-info-container" :class="{ 'user-info-container-mobile': mobileNav }">
        <span @click="goToProfilePage" class="user-avatar-img-container">
          <h2 class="user-avatar-ini2">{{ userAvatar }}</h2>
        </span>
        <h2 class="username-txt" @click="goToProfilePage">{{ userName }}</h2>
    </div>
    <hr class="horizontal-hr">
    <button @click="navigateToServices" class="nvb-btn-style">Services</button>
    <button @click="navigateToPanel" class="nvb-btn-style">Panel</button>
  </div>
  <div :class="{ activeMenu : isActiveMenu }" ref="isActiveMenu">
  </div>
</template>

<script>
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

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
      userName: '',
      userAvatar: '',
    }
  },

    mounted() {
       if (getCookie("token") === '') {
        this.$router.push('/login');
        return;
      }
      window.addEventListener('resize', this.checkScreen);
      window.addEventListener('resize', this.enforceMinWidth);
      this.checkScreen();
      this.enforceMinWidth();
      if (getCookie('AccountType') === 'Area') {
        this.getUserInformation();
      } else if (getCookie('AccountType') === 'Google') {
        this.userName = getCookie('GoogleUsername');
        this.userAvatar = this.userName.charAt(0);
      }
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

    splitString(input) {
      return input.split(';');
    },

      async getUserInformation() {
        try {
          const response = await this.$axios.get(`/auth/userinformation`, {
              headers: {
                  'X-User-Token': getCookie('token'),
              },
          });
          if (response.status === 200) {
            const result = this.splitString(response.data);
            this.userName = result[1];
            this.userAvatar = this.userName.charAt(0);
          }
        } catch (error) {
          if (error.response.status === 400 || error.response.status === 401) {
            removeCookie('token');
            removeCookie('AccountType');
            removeCookie('GoogleUsername');
            this.$router.push('/login');
          }
          console.error(error);
        }
      },

    navigateToServices(event) {
      event.preventDefault()
      this.$router.push('/mobile-services');
    },

    navigateToPanel(event) {
      event.preventDefault()
      this.$router.push('/panel');
    },

    navigateToHome(event) {
      event.preventDefault()
      window.location.href = this.$router.resolve({ name: 'home' }).href;
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

    goToProfilePage(event) {
      event.preventDefault()
      window.location.href = this.$router.resolve({ name: 'profile' }).href;
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

.navbar-container {
height: 5rem;
overflow: hidden;
border-bottom: 1px solid #000000;
box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
backdrop-filter: blur(6px);
}

.header {
  display: flex;
  box-sizing: border-box;
  align-items: center;
  overflow: hidden;
  height: 5rem;
  width: 100%;
  background-color: #bcc1ba;
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
  height: 4.5rem;
  align-items: center;
  left: 24px;
  top: 0;
  transition: .5s ease all;
}

.logo:hover {
  cursor: pointer;
}

.menu {
  width: 3rem;
  height: 3rem;
  margin-left: 1rem;
  margin-right: 0.5rem;
  transition: .5s ease all;
  cursor: pointer;
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
  height: 13.5rem;
  align-items: center;
  flex-direction: column;
  border-bottom: 1px solid #000000;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
  animation:backwards 1s;
  background-color: #bcc1ba;
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
  flex: 1 1 90%;
  min-width: 0;
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

.title:hover {
  cursor: pointer;
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
  z-index: 99;
}

.user-avatar-img-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 4rem;
  height: 4rem;
  border-radius: 50%;
  background-color: #9fb7b9;
  border: 1px solid #969696;
  margin-left: 1rem;
  overflow: hidden;
  cursor: pointer;
}

.user-avatar-img {
  width: 4rem;
  height: 4rem;
}

.username-txt {
  font-size: 2rem;
  font-family: 'inter', sans-serif;
  color: rgba(0, 0, 0, 0.7);
  margin-left: 1rem;
  font-weight: 500;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.username-txt:hover {
  cursor: pointer;
}

.user-info-container {
  display: flex;
  flex-direction: row;
  align-items: center;
  background-color: transparent;
  justify-content: center;
  width: auto;
  height: auto;
  margin-right: 1rem;
  padding: 0;
}

.user-info-container-mobile {
flex-direction: column;
margin-top: 1rem;
margin-bottom: 1rem;
}

.nvb-btn-style {
  width: 100%;
  height: 3rem;
  background-color: transparent;
  border: transparent;
  transition: box-shadow 0.35s ease;
  font-family: 'inter', sans-serif;
  color: rgba(0, 0, 0, 0.7);
  font-size: large;
  cursor: pointer;
}

.nvb-btn-style:active {
  background-color: #c6c9c4;
}

.user-avatar-ini2 {
  font-size: 2rem;
  color: #313030;
  margin: 0;
  padding: 0;
  font-family: 'inter', sans-serif;
  font-weight: 300;
  cursor: pointer;
}

</style>
