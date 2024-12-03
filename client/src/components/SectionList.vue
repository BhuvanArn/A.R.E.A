<template>
    <div class="section-list">
        <ul>
            <li v-for="(section, index) in sections" :key="index">
                {{ section }}
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    name: "SectionList",
    props: {
        sections: {
            type: Array,
            required: true,
        },
    },
};
</script>

<style scoped>
.section-list {
    position: fixed;
    left: 0;
    top: 0;
    width: 200px;
    height: 100vh;
    overflow-y: auto;
    background-color: #f4f4f4;
    border-right: 1px solid #ccc;
    padding: 10px;
}

ul {
    list-style-type: none;
    padding: 0;
}

li {
    padding: 10px;
    cursor: pointer;
    font-size: 16px;
    color: #333;
    border-bottom: 1px solid #ddd;
}

li:hover {
    background-color: #eaeaea;
}
</style>