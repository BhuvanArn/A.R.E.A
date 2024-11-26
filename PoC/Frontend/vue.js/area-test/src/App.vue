<template>
  <nav>
  </nav>
  <div>
  </div>
  <router-view/>
</template>

<script>
import router from './router';

export default {
  name: 'App',
  router
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

</style>
