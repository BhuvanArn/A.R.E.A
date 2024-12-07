<template>
    <div class="widget-list">
        <div class="grid-container">
            <div class="widget" v-for="(widget, index) in widgets" :key="index"
                :style="{ backgroundColor: getColor(index) }">
                {{ widget }}
            </div>
            <div class="widget add-widget" @click="addWidgetInList">
                Add new widget
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "WidgetList",
    methods: {
        getColor(index) {
            const colors = ["#D4A5A5", "#A7C4A0", "#92B4F4", "#E9C46A"];
            return colors[index % colors.length];
        },
        addWidgetInList() {
            this.widgets.push("Widget feur")
        }
    },
    data() {
        return {
            widgets: ["Widget E", "Widget F", "Widget G"],
        };
    }
};
</script>

<style scoped>
.widget-list {
    display: flex;
    flex-direction: column;
    position: fixed;
    right: 0;
    top: 0;
    width: 80vw;
    height: 100vh;
    overflow-y: auto;
}

.grid-container {
    display: grid;
    gap: 10px;
    grid-template-columns: 1fr 1fr 1fr;
    justify-content: start;
    padding: 20px;
    overflow-y: auto;
}

.widget {
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 18px;
    font-weight: bold;
    width: 200px;
    height: 200px;
    color: #fff;
    background-color: #007BFF;
    border-radius: 8px;
    box-shadow: 0px 3px 6px rgba(0, 0, 0, 0.1);
    padding: 20px;
}

.add-widget {
    color: black;
    background-color: white;
}
</style>
