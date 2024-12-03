<template>
    <div class="service-list">
        <h1>Services</h1>
        <ul>
            <li v-for="(section, index) in sections" class=" section-item">
                <div class="service-section-name" :style="{ backgroundColor: getColor(index) }">{{ section }}</div>
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
    methods: {
        getColor(index) {
            const colors = ["#77cbda", "#ff9e99", "#dbcc79", "#8cbd8c"];  //["blue", "red", "yellow", "green"]
            return colors[index % colors.length];
        },
    },
};
</script>

<style scoped>
.service-list {
    position: fixed;
    left: 0;
    top: 0;
    width: 300px;
    height: 100vh;
    overflow-y: auto;
    background-color: #f4f4f4;
    border-right: 1px solid #ccc;
    /* padding: 10px; */
}

.service-section-name {
    /* height: 35px; */
    line-height: 45px;
    padding-left: 30px;
    font-size: 25px;
    font-weight: bold;
    color: white;
    border-radius: 15px;
    box-shadow: 0px 3px rgb(126, 126, 126);
}

h1 {
    text-align: center;
}

ul {
    list-style-type: none;
    padding: 0;
}

li {
    padding: 10px;
    cursor: pointer;
    font-size: 16px;
    /* color: #333; */
    /* border-bottom: 1px solid #ddd; */
}

li:hover {
    background-color: #eaeaea;
}
</style>