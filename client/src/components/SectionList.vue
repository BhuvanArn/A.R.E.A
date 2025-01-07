<template>
    <div class="service-list" v-show="!mobile">
        <h1 class="service-txt-title">Services</h1>
        <!-- <p>&nbsp</p> -->
        <ul>
            <li v-for="(section, index) in services" class="section-item">
                <div class="service-section-name" :style="{ backgroundColor: getColor(index) }">{{ section }}</div>
            </li>
        </ul>
        <div class="add-services" @click="showForm = true">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8 1V15M1 8H15" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            ADD SERVICE
        </div>
    </div>
    <AddServiceModal
      v-if="showForm"
      :services="filteredServices"
      @close="cancelForm"
    />
    <!-- <div v-if="showForm" class="modal-overlay">
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
    </div> -->
</template>

<script>
import AddServiceModal from './AddServiceModal.vue';

export default {

    name: "SectionList",
    components: {
        AddServiceModal,
    },
    mounted() {
        this.checkScreen();
        window.addEventListener('resize', this.checkScreen);
    },
    data() {
        return {
            showForm: false,
            mobile: false,
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
            if (this.services.length === 0) {
                return this.available_services;
            }
            return this.available_services.filter(
                (service) => !this.services.includes(service.name)
            );
        },
    },
    methods: {
        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },
            getColor(index) {
            const colors = ["#77c   bda", "#ff9e99", "#dbcc79", "#8cbd8c"];  //["blue", "red", "yellow", "green"]
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

        async fetchAbout() {
            try {
                const res = await this.$axios.get("/about.json");
                console.log(res);
                this.available_services = res.data.server.services;
            } catch (error) {
                console.error(error);
            }
        },

        async fetchServices() {
            try {
                const token = localStorage.getItem("token");
                const res = await this.$axios.get(`/area/${token}/services`);
                console.log(res);
                if (res.data.services) {
                    this.services = res.data.services;
                } else {
                    this.services = [];
                }
            } catch (error) {
                console.error(error);
            }
        },
    },
    async mounted() {
        // const about_data = {
        //     client: {
        //         host: "10.101.53.35"
        //     },
        //     server: {
        //         current_time: 1531680780,
        //         services: [
        //         {
        //             name: "facebook",
        //             credentials: ["email", "password"],
        //             actions: [
        //                 {
        //                     name: "new_message_in_group",
        //                     description: "A new message is posted in the group",
        //                     inputs: ["Group Link", "Username"]
        //                 },
        //                 {
        //                     name: "new_message_inbox",
        //                     description: "A new private message is received by the user",
        //                     inputs: ["Profile"]
        //                 }
        //             ],
        //             reactions: [
        //                 {
        //                     name: "like_message",
        //                     description: "The user likes a message",
        //                     inputs: ["Profile"]
        //                 }
        //             ]
        //         },
        //         {
        //             "name": "Discord",
        //             "credentials": [
        //                 "token",
        //             ],
        //             actions: [
        //                 {
        //                     name: "new_message_in_group",
        //                     description: "A new message is posted in the group",
        //                     inputs: ["Group Link", "Username"]
        //                 },
        //                 {
        //                     name: "ping_everyone",
        //                     description: "You were pinged by a @everyone",
        //                     inputs: ["Profile"]
        //                 }
        //             ],
        //             reactions: [
        //                 {
        //                     name: "send_message",
        //                     description: "The user sends a message",
        //                     inputs: ["Message"]
        //                 }
        //             ]
        //         },
        //         ]
        //     }
        // };

        // get services from API ABOUT
        await this.fetchAbout();

        // get services already subscribed by the user
        await this.fetchServices();

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
    height: 100%;
    overflow-y: auto;
    background-color: #f4f4f4;
    border-right: 2px solid #888585;
    position: relative;
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
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #46b1c9;
    border-radius: 5px;
    border: none;
    width: 10rem;
    height: 2rem;
    color: #efefef;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 0.5rem;
    margin-bottom: 1rem;
    position: absolute;
    bottom: 1.5vh;
    left: 50%;
    transform: translateX(-50%);
}

.add-services svg {
    margin-right: 0.6rem;
}

.add-services:hover {
    background-color: #3a9cb1;
}

.add-services:active {
    background-color: #2e7f8f;
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

.service-txt-title {
    font-family: 'inter', sans-serif;
    margin: 1rem;
}
</style>
