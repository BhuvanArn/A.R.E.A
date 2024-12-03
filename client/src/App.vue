<template>
  <nav>
    <router-link to="/">Home</router-link> |
    <router-link to="/about">About</router-link>
  </nav>
  <div id="app">
    <DisplayText text="Hello, Vue.js! feur" />
  </div>
  <router-view />
  <p>this is a text</p>
</template>

<script>
import DisplayText from './components/DisplayText.vue';

export default {
  name: "App",
  components: {
    DisplayText,
  },
};
</script>

<style>
nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
