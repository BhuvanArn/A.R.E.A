<template>
    <body>
        <div class="service-list">
            <h1 class="service-txt-title">Services</h1>
            <!-- <p>&nbsp</p> -->
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
    </body>
</template>

<script>
import WidgetList from '@/components/WidgetList.vue';

export default {

    name: "SectionList",
    components: {
        WidgetList,
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
};
</script>


<style scoped>
body {
    margin: 0;
    font-family: Arial, sans-serif;
    display: flex;
    background-color: transparent;
    height: auto;
    width: 100%;
}

.service-list {
    width: 100%;
    height: 100%;
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
    width: 100%;
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

.service-txt-title {
    font-family: 'inter', sans-serif;
    margin: 1rem;
}
</style>
