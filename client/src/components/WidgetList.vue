<template>
    <div class="widget-list">
        <div class="grid-container">
            <div class="widget" v-for="(widget, index) in user_widgets" :key="index">
                <div>
                    <div class="widget-title">
                        <h2>Widget {{ index + 1 }}</h2>
                    </div>
                    <div class="widget-action">
                        <h3>Trigger: {{ widget.action.name }}</h3>
                        <p>Service: {{ services[widget.action.service_id] }}</p>
                        <p>&nbsp</p>
                        <p>{{ widget.action.description }}</p>
                    </div>
                    <div class="widget-reaction">
                        <h3>Reaction: {{ widget.reaction.name }}</h3>
                        <p>Service: {{ services[widget.reaction.service_id] }}</p>
                        <p>&nbsp</p>
                        <p>{{ widget.reaction.description }}</p>
                    </div>
                </div>
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
            this.user_widgets.push({
                action: {
                    name: "New Action",
                    description: "New action description",
                },
                reaction: {
                    name: "New Reaction",
                    description: "New reaction description",
                },
            });
        },
    },
    data() {
        return {
            services: {},
            user_widgets: [],
        };
    },
    mounted() {
        // fetch("URL_TO_API")
        //     .then((response) => response.json())
        //     .then((data) => {
        //         // Update the user_widgets array with the API data
        //         this.user_widgets = data;
        //     })
        //     .catch((error) => {
        //         console.error("Error fetching user_widgets:", error);
        //     });
        this.services["1"] = "Discord";
        this.services["2"] = "Google";
        this.user_widgets = [
        {
            action: {
                id: "1",
                service_id: "2",
                name: "Action 1",
                description: "Description de l'action 1",
            },
            reaction: {
                id: "1",
                service_id: "1",
                name: "Reaction 1",
                description: "Description de la reaction 1",
            },
        },
        {
            action: {
                id: "1",
                service_id: "1",
                name: "Action 2",
                description: "Description de l'action 2",
            },
            reaction: {
                id: "1",
                service_id: "1",
                name: "Reaction 2",
                description: "Description de la reaction 2",
            },
        },
    ];

    },
};
</script>

<style scoped>
.widget-list {
    display: flex;
    height: 100vh;
    overflow-y: auto;
}

.grid-container {
    display: grid;
    width: 80vw;
    gap: 10px;
    grid-template-columns: 1fr 1fr 1fr;
    padding: 20px;
    overflow-y: auto;
}

.widget {
    display: flex;
    flex-direction: column;
    /* align-items: center;
    justify-content: center;*/
    font-size: 18px;
    font-weight: bold;
    text-align: center;
    color: #000;
    width: 300px;
    height: 400px;
    background-color: #fff;
    border-radius: 8px;
    outline: 2px solid black;
    box-shadow: 0px 3px 6px rgba(0, 0, 0, 0.1);
}

.widget-title {
    height: 50px;
    background-color: #bcc1ba;
    padding: 5px;
}

.widget-action {
    display: flex;
    flex-direction: column;
    flex: 1;
    padding: 20px;
    outline: 1px solid black;
}

.widget-reaction {
    display: flex;
    flex-direction: column;
    flex: 1;
    padding: 20px;
}

.add-widget {
    color: black;
    background-color: white;
}
</style>
