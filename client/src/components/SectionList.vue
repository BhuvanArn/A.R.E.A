<template>
    <div class="service-list">
        <h1>Services</h1>
        <p>&nbsp</p>
        <ul>
            <li v-for="(section, index) in services" class=" section-item">
                <div class="service-section-name" :style="{ backgroundColor: getColor(index) }">{{ section }}</div>
            </li>
        </ul>
        <div class="add-services" @click="showForm = true">
            ADD SERVICE
        </div>
        <div v-if="showForm" class="modal-overlay">
            <div class="modal">
                <h3>Add New Service</h3>
                <p>&nbsp;</p>
                <div v-if="filteredServices.length === 0">
                    No other services available
                    <p>&nbsp;</p>
                    <button type="button" @click="cancelForm">Cancel</button>
                </div>
                <div v-else>
                    <form @submit.prevent="createService">
                        <div>
                            <label for="service">Services: </label>
                            <select id="service" v-model="selectedService">
                                <option v-for="(service, id) in filteredServices" :key="id" :value="id">
                                    {{ service.name }}
                                </option>
                            </select>
                            <p>&nbsp;</p>
                            <label for="credentials">Credentials: </label>
                            <div v-if="selectedService !== null">
                                <div v-for="(credential) in filteredServices[selectedService]?.credentials || []"
                                    :key="index">
                                    <label for="credential">{{ credential }}: </label>
                                    <input id="credential" type="text" v-model="credentials[credential]"
                                        placeholder="Enter value" />
                                </div>
                            </div>
                        </div>
                        <button type="submit">Create Service</button>
                        <button type="button" @click="cancelForm">Cancel</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <WidgetList></WidgetList>
</template>

<script>
import WidgetList from '@/components/WidgetList.vue';

export default {

    name: "SectionList",
    components: {
        WidgetList,
    },
    data() {
        return {
            showForm: false,
            selectedService: null,
            credentials: {},
            available_services: [
                {
                    "name": "Discord",
                    "credentials": [
                        "token",
                    ],
                },
                {
                    "name": "Google",
                    "credentials": [
                        "email",
                        "password",
                    ]
                }
            ],
            services: [
                // "Discord",
            ],
        }
    },
    computed: {
        filteredServices() {
            return this.available_services.filter(
                (service) => !this.services.includes(service.name)
            );
        },
    },
    methods: {
        getColor(index) {
            const colors = ["#77cbda", "#ff9e99", "#dbcc79", "#8cbd8c"];  //["blue", "red", "yellow", "green"]
            return colors[index % colors.length];
        },
        createService() {
            if (this.selectedService != null &&
                Object.keys(this.credentials).length == Object.keys(this.filteredServices[this.selectedService].credentials).length) {
                this.services.push(this.filteredServices[this.selectedService]["name"]);
                this.resetForm();
            } else {
                alert("Please select a service and specify the corresponding credentials.");
            }

            // TODO : Send credentials to API
            alert(JSON.stringify(this.credentials, null, 2));

            this.credentials = {};
        },
        cancelForm() {
            this.resetForm();
        },
        resetForm() {
            this.showForm = false;
            this.selectedServiceId = null;
        },
    },
    mounted() {
        const about_data = {
            client: {
                host: "10.101.53.35"
            },
            server: {
                current_time: 1531680780,
                services: [
                {
                    name: "facebook",
                    credentials: ["email", "password"],
                    actions: [
                        {
                            name: "new_message_in_group",
                            description: "A new message is posted in the group",
                            inputs: ["Group Link", "Username"]
                        },
                        {
                            name: "new_message_inbox",
                            description: "A new private message is received by the user",
                            inputs: ["Profile"]
                        }
                    ],
                    reactions: [
                        {
                            name: "like_message",
                            description: "The user likes a message",
                            inputs: ["Profile"]
                        }
                    ]
                },
                {
                    "name": "Discord",
                    "credentials": [
                        "token",
                    ],
                    actions: [
                        {
                            name: "new_message_in_group",
                            description: "A new message is posted in the group",
                            inputs: ["Group Link", "Username"]
                        },
                        {
                            name: "ping_everyone",
                            description: "You were pinged by a @everyone",
                            inputs: ["Profile"]
                        }
                    ],
                    reactions: [
                        {
                            name: "send_message",
                            description: "The user sends a message",
                            inputs: ["Message"]
                        }
                    ]
                },
                ]
            }
        };
        localStorage.setItem("available_services", JSON.stringify(about_data.server.services));
        this.available_services = about_data.server.services;

        // GET /users/{user_token}/services
        const user_services = {
            services: [
                "Discord",
                "Google",
            ]
        }
        localStorage.setItem("user_services", JSON.stringify(user_services));

        for (let i = 0; i < user_services.services.length; i++) {
            // GET /users/{user_token}/services/{service_name}/actions_reactions
            // fetched_data = ...
            // user_actions_reactions[user_services.services[i]] = fetched_data
        }

        const user_actions_reactions =
        {
            "facebook": {
                actions: [
                    {
                        name: "new_message_in_group",
                        description: "A new message is posted in the group",
                        inputs: ["Group Link", "Username"]
                    },
                ],
                reactions: [
                    {
                        name: "like_message",
                        action: "new_message_in_group",
                        description: "The user likes a message",
                        inputs: ["Profile"]
                    }
                ]
            },
            "discord": {
                actions: [
                    {
                        name: "ping_everyone",
                        description: "You were pinged by a @everyone",
                        inputs: ["Profile"]
                    }
                ],
                reactions: [
                    {
                        name: "send_message",
                        action: "ping_everyone",
                        description: "The user sends a message",
                        inputs: ["Message"]
                    }
                ]
            }
        }
        localStorage.setItem("user_actions_reactions", JSON.stringify(user_actions_reactions));
    }
};
</script>

<style scoped>
.service-list {
    width: 20vw;
    height: 100vh;
    overflow-y: auto;
    background-color: #f4f4f4;
    border-right: 1px solid #ccc;
}

.service-section-name {
    line-height: 45px;
    padding-left: 30px;
    font-size: 25px;
    font-weight: bold;
    color: white;
    border-radius: 15px;
    box-shadow: 0px 3px rgb(126, 126, 126);
    cursor: pointer;
}

.service-section-name:hover {
    color: black;
}

.add-services {
    width: 20vw;
    text-align: center;
    font-weight: bold;
    font-size: 20px;
    padding: 20px;
    box-shadow: 0 -1px 0 #000;
    position: absolute;
    bottom: 0;
    left: 0;
    cursor: pointer;
}

.add-services:hover {
    background-color: #bcc1ba;
}


/* Modal Styles */
.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    /* Semi-transparent background */
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
    /* Ensures it's above everything else */
}

.modal {
    background-color: white;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.25);
    width: 400px;
    max-width: 90%;
}

.modal h3 {
    margin-top: 0;
}

.modal form {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.modal button {
    margin-top: 10px;
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
}
</style>
