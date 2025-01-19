<template>
    <div class="service-list" v-show="!mobile">
        <h1 class="service-txt-title">Services</h1>
        <ul>
            <li v-for="(section, _) in services" class="section-item">
                <div class="service-section-name" :style="{ backgroundColor: getBrandColor(section.name) }" @click="selectedService = section; openServiceDetails()">
                    <div class="icon-square">
                        <Iconify :icon="getServiceIcon(section.name)" class="service-icon-small" />
                    </div>
                    <div class="service-name">
                        {{ section.name }}
                    </div>
                </div>
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
</template>

<script>
import AddServiceModal from './AddServiceModal.vue';
import { Icon } from "@iconify/vue";
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

export default {

    name: "SectionList",
    components: {
        AddServiceModal,
        Iconify: Icon
    },
    mounted() {
    },
    data() {
        return {
            showForm: false,
            mobile: false,
            selectedService: null,
            credentials: {},
            available_services: [],
            services: [
                // "Discord",
            ],
            selectedService: null,
        }
    },
    computed: {
        filteredServices() {
            if (this.services.length === 0) {
                return this.available_services;
            }
            return this.available_services.filter(
                (service) => !this.services.some((s) => s.name === service.name)
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
        getBrandColor(name) {
            switch (name) {
                case "discord":
                    return  "#7289da";
                case "google":
                    return "#4285f4";
                case "twitter":
                    return "#1da1f2";
                case "github":
                    return "#24292e";
                case "spotify":
                    return "#1db954";
                case "dropbox":
                    return "#0061ff";
                case "reddit":
                    return "#ff4500";
                default:
                    return "gray";
            }
        },

        getServiceIcon(name) {
            const lowerCased = name.toLowerCase();

            if (lowerCased === "test") {
                return "mdi:robot";
            }
            return `mdi:${lowerCased}`;
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
                this.available_services = res.data.server.services;
            } catch (error) {
                console.error(error);
            }
        },

        async fetchServices() {
            try {
                const token = getCookie('token');
                const res = await this.$axios.get(`/area/services/false`, {
                    headers: {
                        'X-User-Token': token,
                    },
                });
                if (res.data) {
                    this.services = res.data;
                } else {
                    this.services = [];
                }
            } catch (error) {
                console.error(error);
                this.services = [];
            }
        },

        openServiceDetails() {
            // Redirect to service details page
            this.$router.push({ name: 'service-details', params: { serviceName: this.selectedService.name } });
        },
    },
    async mounted() {

        // get services from API ABOUT
        await this.fetchAbout();

        // get services already subscribed by the user
        await this.fetchServices();
    }
};
</script>


<style scoped>

.service-list {
    width: 100%;
    height: 100%;
    overflow-y: auto;
    background-color: #f4f4f4;
    border-right: 2px solid #888585;
    position: relative;
}

.service-section-name {
    display: flex;
    align-items: center;
    line-height: 45px;
    font-size: 25px;
    font-weight: bold;
    color: white;
    border-radius: 15px;
    cursor: pointer;
    text-align: center;
    padding: 10px;
}

.service-section-name:hover {
    filter: brightness(1.1);
}

.icon-square {
    width: 30px;
    height: 30px;
    background-color: #f4f4f4;
    border: 2px solid #e8e7e4;
    border-radius: 5px;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-left: 10px;
    flex-shrink: 0;
}

.service-icon-small {
    color: black;
    font-size: 20px;
    margin-right: 1px;
}

.service-name {
    font-size: 1.25rem;
    font-weight: 600;
    margin-right: 10px;
    flex-grow: 1;
}

.section-item {
    margin: 0 0.5rem;
    padding: 16px 50px;
}

.add-services {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #28728B;
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

.service-txt-title {
    text-align: center;
    padding-top: 0.5rem;
}
</style>
