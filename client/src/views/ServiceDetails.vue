<template>
    <div class="main-page">
        <NavBar class="navbar"></NavBar>
        <div :style="{ backgroundColor: serviceColor }" class="service-details-page">
          <div class="service-header">
            <Iconify :icon="serviceIcon" class="service-icon" />
            <h1 class="service-title">{{ serviceName.slice(0, 1).toUpperCase() + serviceName.slice(1) }}</h1>
          </div>
          <div class="service-content">
            <h4 class="area-title">Actions & Reactions</h4>
            <div class="actions-reactions-list">
              <div v-for="action in service.actions" :key="action.name" class="action-reaction-card">
                <div class="type-section">
                  <small>Action</small>
                </div>
                <div class="card-content">
                  <div class="icon-section">
                    <div class="icon-square">
                      <Iconify :icon="serviceIcon" class="service-icon-small" />
                    </div>
                  </div>
                  <div class="info-section">
                    <strong>{{ truncateText(action.name, 30) }}</strong>
                    <p>{{ truncateText(action.description, 50) }}</p>
                  </div>
                </div>
              </div>
              <div v-for="reaction in service.reactions" :key="reaction.name" class="action-reaction-card">
                <div class="type-section">
                  <small>Reaction</small>
                </div>
                <div class="card-content">
                  <div class="icon-section">
                    <div class="icon-square">
                      <Iconify :icon="serviceIcon" class="service-icon-small" />
                    </div>
                  </div>
                  <div class="info-section">
                    <strong>{{ truncateText(reaction.name, 30) }}</strong>
                    <p>{{ truncateText(reaction.description, 50) }}</p>
                  </div>
                </div>
              </div>
            </div>
            <button @click="toggleSubscription" class="common-btn">
              {{ isSubscribed ? 'Deactivate' : 'Activate' }}
            </button>
            <button @click="goBack" class="common-btn back-btn">
              Back to Panel
            </button>
          </div>
        </div>
     </div>
  </template>

<script>
import { Icon } from "@iconify/vue";
import NavBar from "@/components/WorkspaceNavBar.vue";
import { getCookie } from '@/utils/cookies';

export default {
    name: "ServiceDetails",
    components: {
        Iconify: Icon,
        NavBar,
    },
    data() {
        return {
        serviceName: this.$route.params.serviceName,
        service: {},
        serviceIcon: "",
        serviceColor: "",
        isSubscribed: false,
        };
    },
    async mounted() {
        await this.fetchServiceDetails();
        await this.checkSubscriptionStatus();
    },
    methods: {
        async fetchServiceDetails() {
        try {
            const res = await this.$axios.get("/about.json");

            const service = res.data.server.services.find(
            (s) => s.name.toLowerCase() === this.serviceName.toLowerCase());
            this.service = service;
            this.serviceIcon = `mdi:${service.name.toLowerCase()}`;
            if (service.name.toLowerCase() === "test") {
            this.serviceIcon = "mdi:robot";
            }
            this.serviceColor = this.getBrandColor(service.name);
        } catch (error) {
            console.error(error);
        }
        },
        async checkSubscriptionStatus() {
        try {
            const token = getCookie('token');
            const res = await this.$axios.get(`/area/services/false`, {
            headers: {
                "X-User-Token": token,
            },
            });
            this.isSubscribed = res.data.some(
            (s) => s.name.toLowerCase() === this.serviceName.toLowerCase()
            );
        } catch (error) {
            console.error(error);
        }
        },
        toggleSubscription() {
        if (this.isSubscribed) {
            this.deactivateService();
        } else {
            this.activateService();
        }
        },
        async activateService() {
        try {
            const token = getCookie('token');
            await this.$axios.post(
            `/area/subscribe_service`,
            { name: this.serviceName },
            {
                headers: {
                "X-User-Token": token,
                },
            }
            );
            this.isSubscribed = true;
        } catch (error) {
            console.error(error);
        }
        },
        async deactivateService() {
        try {
            const token = getCookie('token');
            await this.$axios.post(
            `/area/unsubscribe_service`,
            { name: this.serviceName },
            {
                headers: {
                "X-User-Token": token,
                },
            }
            );
            this.isSubscribed = false;
        } catch (error) {
            console.error(error);
        }
        },
        getBrandColor(name) {
        switch (name.toLowerCase()) {
            case "discord":
            return "#7289da";
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
        truncateText(text, maxLength) {
        if (text.length > maxLength) {
            return text.slice(0, maxLength) + "...";
        }
        return text;
        },

        goBack() {
            this.$router.push({ name: 'panel' }); // Adjust the route name as needed
        }
    },
};
</script>

<style scoped>

.main-page {
    font-family: 'inter', sans-serif;
    position: relative;
    z-index: 0;
    height: 100%;
    overflow: hidden;
}

.service-details-page {
    height: calc(100vh - 5rem);
    outline: none;
    position: relative;
    z-index: -1;
    display: flex;
    background-color: #efefef;
    color: white;
    flex-direction: column;
}

.service-header {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-bottom: 2rem;
    margin-top: 2rem;
}

.service-icon {
    font-size: 8rem;
    margin-bottom: 1rem;
}

.service-title {
    font-size: 2.5rem;
    font-weight: bold;
    margin-top: -1.5rem;
}

.service-content {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
}

.area-title {
    font-size: 1.5rem;
    font-weight: bold;
    margin-bottom: 1rem;
}

.actions-reactions-list {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 90%;
    margin-bottom: 2rem;
    max-height: 40vh;
    overflow-y: auto;
}

.action-reaction-card {
    display: flex;
    flex-direction: column;
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 10px;
    margin-bottom: 10px;
    background-color: white;
    color: black;
    width: 80%;
    position: relative;
}

.action-reaction-card:hover {
    background-color: #e4e4e4;
}

.type-section {
    position: absolute;
    top: 10px;
    right: 10px;
    background-color: #f4f4f4;
    padding: 2px 5px;
    border-radius: 3px;
    font-size: 0.9rem;
}

.card-content {
    display: flex;
    align-items: center;
}

.icon-section {
    display: flex;
    align-items: center;
    margin-right: 10px;
    margin-bottom: 3px;
}

.icon-square {
    width: 48px;
    height: 48px;
    background-color: #f4f4f4;
    border: 2px solid #e8e7e4;
    border-radius: 5px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.service-icon-small {
    font-size: 32px;
}

.info-section {
    flex-grow: 1;
}

.common-btn {
    background-color: #28728b;
    color: #efefef;
    border-radius: 5px;
    border: none;
    width: 8rem;
    height: 2rem;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 1rem;
}

.common-btn:hover {
    background-color: #3a9cb1;
}

.common-btn:active {
    background-color: #2e7f8f;
}

.back-btn {
    background-color: #555;
}

.back-btn:hover {
    background-color: #777;
}

.back-btn:active {
    background-color: #444;
}

</style>
