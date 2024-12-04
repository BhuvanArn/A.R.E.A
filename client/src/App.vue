<template>
  <div id="app">
    <SectionList :sections="sectionNames" />
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
      sectionNames: [],
    };
  },
  mounted() {
    this.fetchSections();
  },
  methods: {
    fetchSections() {
      // TODO Fetch services from server here
      this.sectionNames = ["Google", "Gmail", "Discord", "Timer", "Outlook"];
    },
  },

};
</script>

<style>
.app {
  display: flex;
}
</style>
