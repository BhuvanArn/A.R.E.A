<template>
  <div>
    <p>{{ text }}</p>
  </div>
</template>

<script>
export default {
  name: "DisplayText",
  props: {
    text: {
      type: String,
      required: true,
    },
  },
};
</script>

<style scoped>
p {
  font-size: 16px;
  color: #333;
}
</style>