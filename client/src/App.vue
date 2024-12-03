<template>
  <div id="app">
    <SectionList :sections="sectionNames" />
    <div class="content">
      <h1>Welcome to My Page</h1>
      <p>Content goes here...</p>
    </div>
  </div>
</template>

<script>
import SectionList from './components/SectionList.vue';

export default {
  name: "App",
  components: {
    SectionList,
  },
  data() {
    return {
      sectionNames: ["Introduction", "Features", "Pricing", "Contact Us", "FAQ"],
    };
  },
};
</script>

<style>
#app {
  display: flex;
}

.content {
  margin-left: 220px;
  /* Offset for the fixed section list */
  padding: 20px;
}
</style>
