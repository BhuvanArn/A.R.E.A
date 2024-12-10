<template>
    <div class="widget-list">
        <div class="grid-container">
            <!-- Existing widgets -->
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

            <!-- Add new widget button -->
            <div class="widget add-widget" @click="showForm = true">
                Add new widget
            </div>
        </div>

        <!-- Dynamic form for creating a new widget -->
        <div v-if="showForm" class="widget-form">
            <h3>Create New Widget</h3>
            <form @submit.prevent="createWidget">
                <div>
                    <label for="service">Service:</label>
                    <select id="service" v-model="selectedServiceId">
                        <option v-for="(service, id) in services" :key="id" :value="id">
                            {{ service }}
                        </option>
                    </select>
                </div>
                <div v-if="selectedServiceId">
                    <label for="action">Action:</label>
                    <select id="action" v-model="selectedAction">
                        <option v-for="action in all_widgets[selectedServiceId]?.actions || []" :key="action.id"
                            :value="action">
                            {{ action.name }}
                        </option>
                    </select>
                </div>
                <div v-if="selectedServiceId">
                    <label for="reaction">Reaction:</label>
                    <select id="reaction" v-model="selectedReaction">
                        <option v-for="reaction in all_widgets[selectedServiceId]?.reactions || []" :key="reaction.id"
                            :value="reaction">
                            {{ reaction.name }}
                        </option>
                    </select>
                </div>
                <button type="submit">Create Widget</button>
                <button type="button" @click="cancelForm">Cancel</button>
            </form>
        </div>
    </div>
</template>

<script>
export default {
    name: "WidgetList",
    data() {
        return {
            services: {
                "1": "Discord",
                "2": "Google",
            },
            user_widgets: [
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
            ],
            all_widgets: {
                "1": {
                    actions: [
                        {
                            id: "1",
                            name: "Action 1 discord",
                            description: "Description de l'action 1",
                        },
                    ],
                    reactions: [
                        {
                            id: "1",
                            name: "Reaction 1 discord",
                            description: "Description de la reaction 1",
                        },
                    ],
                },
                "2": {
                    actions: [
                        {
                            id: "1",
                            name: "Action 1 google",
                            description: "Description de l'action 1",
                        },
                    ],
                    reactions: [
                        {
                            id: "1",
                            name: "Reaction 1 google",
                            description: "Description de la reaction 1",
                        },
                    ],
                },
            },
            showForm: false,
            selectedServiceId: null,
            selectedAction: null,
            selectedReaction: null,
        };
    },
    methods: {
        createWidget() {
            if (this.selectedAction && this.selectedReaction) {
                this.user_widgets.push({
                    action: {
                        ...this.selectedAction,
                        service_id: this.selectedServiceId,
                    },
                    reaction: {
                        ...this.selectedReaction,
                        service_id: this.selectedServiceId,
                    },
                });
                this.resetForm();
            } else {
                alert("Please select both an action and a reaction.");
            }
        },
        cancelForm() {
            this.resetForm();
        },
        resetForm() {
            this.showForm = false;
            this.selectedServiceId = null;
            this.selectedAction = null;
            this.selectedReaction = null;
        },
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
    align-items: center;
    text-align: center;
    padding: 10px;
    cursor: pointer;
    border: 1px solid black;
    border-radius: 5px;
}

.widget-form {
    padding: 20px;
    background-color: #f8f9fa;
    border: 1px solid #ccc;
    border-radius: 5px;
    max-width: 400px;
    margin: 20px auto;
}

.widget-form form {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.widget-form button {
    margin-top: 10px;
}
</style>
