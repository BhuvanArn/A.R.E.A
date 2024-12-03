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
      sectionNames: ["Google", "Gmail", "Discord", "Timer", "Outlook"],
    };
  },
};
</script>

<style>
.app {
  display: flex;
}
</style>
