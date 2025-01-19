<template>
    <div class="modal-overlay" @click.self="closeModal">
        <div class="modal">
            <div class="modal-header">
                <h2 class="modal-title" :class="{ 'modal-title-mobile': mobile }">Subscribe to {{ selectedService ?
                    selectedService.name.slice(0, 1).toUpperCase() + selectedService.name.slice(1) : "a Service" }}</h2>
                <img src="@/assets/logo.png" class="logo" :class="{ 'logo-mobile': mobile }" alt="logo" />
            </div>
            <!-- Step 1: Carousel of Services -->
            <div v-if="step === 1" class="carousel-container">
                <div v-if="services.length === 0" class="no-services">
                    No new services available
                </div>
                <div v-else-if="services.length <= 3" class="carousel-container">
                    <div class="service-card" v-for="(service, idx) in services" :key="idx"
                        @click="viewService(service)" :style="{ '--bg-color': getBrandColor(service.name) }">
                        <Iconify :icon="getServiceIcon(service.name)" />
                        <h4>{{ service.name.slice(0, 1).toUpperCase() + service.name.slice(1) }}</h4>
                        <hr class="divider" />
                        <p>{{ service.actions.length }} Action{{ service.actions.length > 1 ? 's' : '' }}</p>
                        <p>{{ service.reactions.length }} Reaction{{ service.reactions.length > 1 ? 's' : '' }}</p>
                    </div>
                </div>
                <div v-else class="carousel-container">
                    <!-- left arrow SVG -->
                    <button class="nav-arrow" @click="prevService">
                        <svg fill="#000000" height="16px" width="16px" viewBox="-33 -33 396 396"
                            transform="rotate(180)">
                            <path
                                d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606C255,161.018,253.42,157.202,250.606,154.389z" />
                        </svg>
                    </button>
                    <div class="service-card" v-for="(service, idx) in visibleServices" :key="idx"
                        @click="viewService(service)"
                        :style="{ '--bg-color': getBrandColor(service.name), '--text-color': getComplementaryColor(getBrandColor(service.name)) }">
                        <Iconify :icon="getServiceIcon(service.name)" :style="{ color: 'var(--text-color)' }" />
                        <h4 :style="{ color: 'var(--text-color)' }">{{ service.name.slice(0, 1).toUpperCase() +
                            service.name.slice(1) }}</h4>
                        <hr class="divider" />
                        <p :style="{ color: 'var(--text-color)' }">{{ service.actions.length }} Action{{
                            service.actions.length > 1 ? 's' : '' }}</p>
                        <p :style="{ color: 'var(--text-color)' }">{{ service.reactions.length }} Reaction{{
                            service.reactions.length > 1 ? 's' : '' }}</p>
                    </div>
                    <!-- right arrow SVG -->
                    <button class="nav-arrow" @click="nextService">
                        <svg fill="#000000" height="16px" width="16px" viewBox="-33 -33 396 396">
                            <path
                                d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606C255,161.018,253.42,157.202,250.606,154.389z" />
                        </svg>
                    </button>
                </div>
            </div>

            <!-- Step 2: Service Overview -->
            <div v-else-if="step === 2" class="service-overview">
                <div class="service-info"
                    :style="{ '--bg-color': getBrandColor(selectedService.name), '--text-color': getComplementaryColor(getBrandColor(selectedService.name)) }">
                    <Iconify :icon="getServiceIcon(selectedService.name)" class="service-icon"
                        :style="{ color: 'var(--text-color)' }" />
                    <h3 :style="{ color: 'var(--text-color)' }">{{ selectedService.name.slice(0, 1).toUpperCase() +
                        selectedService.name.slice(1) }}</h3>
                    <button class="common-btn" @click="openServiceDetails">MORE DETAILS</button>
                    <div v-if="showForm" class="service-credentials">
                        <input type="email" v-model="credentials.email" placeholder="Email" />
                        <input type="password" v-model="credentials.password" placeholder="Password" />
                        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
                        <button class="common-btn" @click="authDiscord">AUTHORIZE</button>
                    </div>
                    <button v-else class="common-btn" @click="activateService">ACTIVATE</button>
                </div>
                <!-- Quick listing of actions/reactions -->
                <div class="service-details">
                    <h4>Supported actions and reactions</h4>
                    <div class="actions-reactions-list">
                        <div v-for="action in selectedService.actions" :key="action.name" class="action-reaction-card">
                            <div class="type-section">
                                <small>Action</small>
                            </div>
                            <div class="card-content">
                                <div class="icon-section">
                                    <div class="icon-square">
                                        <Iconify :icon="getServiceIcon(selectedService.name)"
                                            class="service-icon-small" />
                                    </div>
                                </div>
                                <div class="info-section">
                                    <strong>{{ truncateText(action.name, 30) }}</strong>
                                    <p>{{ truncateText(action.description, 50) }}</p>
                                </div>
                            </div>
                        </div>
                        <div v-for="reaction in selectedService.reactions" :key="reaction.name"
                            class="action-reaction-card">
                            <div class="type-section">
                                <small>Reaction</small>
                            </div>
                            <div class="card-content">
                                <div class="icon-section">
                                    <div class="icon-square">
                                        <Iconify :icon="getServiceIcon(selectedService.name)"
                                            class="service-icon-small" />
                                    </div>
                                </div>
                                <div class="info-section">
                                    <strong>{{ truncateText(reaction.name, 30) }}</strong>
                                    <p>{{ truncateText(reaction.description, 50) }}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { Icon } from "@iconify/vue";
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

export default {
    name: "AddServiceModal",
    components: {
        Iconify: Icon
    },
    props: {
        show: Boolean,
        services: Array
    },
    data() {
        return {
            step: 1,
            selectedService: null,
            indexStart: 0,
            maxVisible: 3,
            credentials: {},
            showForm: false,
            errorMessage: '',
        };
    },
    computed: {
        visibleServices() {
            if (this.services.length <= this.maxVisible) {
                return this.services;
            }
            // do wrap-around
            const result = [];
            for (let i = 0; i < this.maxVisible; i++) {
                const idx = (this.indexStart + i) % this.services.length;
                result.push(this.services[idx]);
            }
            return result;
        }
    },
    methods: {
        closeModal() {
            this.$emit('close');
        },
        // Step 1: Carousel
        nextService() {
            this.indexStart = (this.indexStart + 1) % this.services.length;
        },
        prevService() {
            this.indexStart = (this.indexStart + this.services.length - 1) % this.services.length;
        },
        viewService(service) {
            this.selectedService = service;
            this.step = 2;
        },
        getServiceIcon(name) {
            const lowerCased = name.toLowerCase();

            if (name === "test") {
                return "mdi:robot";
            }
            return `mdi:${lowerCased}`;
        },
        openServiceDetails() {
            // Redirect to service details page
            this.$router.push({ name: 'service-details', params: { serviceName: this.selectedService.name } });
        },
        async activateService() {

            try {
                const token = getCookie('token');

                var serviceToken = null;

                if (this.selectedService.name == "discord") {
                    this.showForm = true;
                    return;
                }
                if (this.selectedService.name == "spotify") {
                    await this.authSpotify();
                    return;
                }
                if (this.selectedService.name == "dropbox") {
                    await this.authDropbox();
                    return;
                }
                if (this.selectedService.name == "github") {
                    await this.authGithub();
                    return;
                }
                if (this.selectedService.name == "reddit") {
                    await this.authReddit();
                    return;
                }

                const res = await this.$axios.post(`/area/subscribe_service`, {
                    name: this.selectedService.name,
                    auth: serviceToken || null,
                }, {
                    headers: {
                        'X-User-Token': token,
                    },
                });
                console.log(res);

                // Refresh page so that the new service is displayed
                this.$router.go();
            } catch (error) {
                console.error(error);
            } finally {
                if (!this.showForm) {
                    this.closeModal();
                }
            }
        },

        getBrandColor(name) {
            switch (name) {
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

        getComplementaryColor(hex) {
            // Convert hex to RGB
            const r = parseInt(hex.slice(1, 3), 16);
            const g = parseInt(hex.slice(3, 5), 16);
            const b = parseInt(hex.slice(5, 7), 16);

            // Calculate brightness (YIQ formula)
            const brightness = (r * 299 + g * 587 + b * 114) / 1000;

            // Return black for light colors and white for dark colors
            return brightness > 128 ? '#000000' : '#FFFFFF';
        },

        truncateText(text, maxLength) {
            if (text.length > maxLength) {
                return text.slice(0, maxLength) + '...';
            }
            return text;
        },


        // each service Oauth

        async authSpotify() {
            try {
                // get the token from spotify
                const res = await this.$axios.post(`/oauth/spotify/authorize`,
                    {
                        "scopes": "user-read-private user-read-playback-state",
                        "redirect_url": "http://localhost:8081/oauth/spotify/callback"
                    });

                // res.data contains "authorize_url" which is the url to redirect the user to
                window.open(res.data.authorize_url, "_blank");

                // this call will open a new tab to the spotify login page,
                // then the user will be redirected to the callback url
            } catch (error) {
                console.error(error);
            };
        },

        async authDropbox() {
            try {
                // get the token from dropbox
                const res = await this.$axios.post(`/oauth/dropbox/authorize`,
                    {
                        "redirect_url": "http://localhost:8081/oauth/dropbox/callback"
                    });

                // res.data contains "authorize_url" which is the url to redirect the user to
                window.open(res.data.authorize_url, "_blank");

                // this call will open a new tab to the dropbox login page,
                // then the user will be redirected to the callback url
            } catch (error) {
                console.error(error);
            };
        },

        async authGithub() {
            try {
                // get the token from github
                const res = await this.$axios.post(`/oauth/github/authorize`,
                    {
                        "scopes": "repo",
                        "redirect_url": "http://localhost:8081/oauth/github/callback"
                    });

                // res.data contains "authorize_url" which is the url to redirect the user to
                window.open(res.data.authorize_url, "_blank");

                // this call will open a new tab to the github login page,
                // then the user will be redirected to the callback url
            } catch (error) {
                console.error(error);
            };
        },

        async authReddit() {
            try {
                // get the token from reddit
                const res = await this.$axios.post(`/oauth/reddit/authorize`,
                    {
                        "scopes": "read submit subscribe",
                        "redirect_url": "http://localhost:8081/oauth/reddit/callback"
                    });

                // res.data contains "authorize_url" which is the url to redirect the user to
                window.open(res.data.authorize_url, "_blank");

                // this call will open a new tab to the reddit login page,
                // then the user will be redirected to the callback url
            } catch (error) {
                console.error(error);
            };
        },

        async authDiscord() {
            try {

                // get the token from discord
                const res = await this.$axios.post(`/auth/discord-login`,
                    {
                        "Email": this.credentials.email,
                        "Password": this.credentials.password
                    });

                const token = res.data;
                const user_token = getCookie('token');

                // subscribe to the service
                const subscribed_res = await this.$axios.post(`/area/subscribe_service`, {
                    name: this.selectedService.name,
                    auth: {
                        token: token
                    }
                }, {
                    headers: {
                        'X-User-Token': user_token,
                    },
                });

                this.showForm = false;

                // Refresh page so that the new service is displayed
                this.$router.go();
            } catch (error) {
                console.error(error);
                this.errorMessage = "Invalid credentials";
            };
        },
    }
};
</script>

<style scoped>
.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
}

.modal {
    background-color: #f4f4f4;
    padding: 20px;
    border-radius: 8px;
    width: 1000px;
    max-width: 90%;
    border: 8px solid #bcc1ba;
    border-radius: 15px;
    box-shadow: 0 15px 50px rgba(0, 0, 0, 0.9);
    padding: 0;
}

.modal-header {
    border-bottom: 2px solid black;
    padding: 1rem;
    height: 100%;
    border-top-left-radius: 7px;
    border-top-right-radius: 7px;
    margin-bottom: 1rem;

    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    background-color: #bcc1ba;
}

.modal-title {
    margin: 0;
    font-family: 'inter', sans-serif;
    font-weight: 400;
    font-size: 2.5rem;
    color: #313030;
}

.modal-title-mobile {
    font-size: 1.3rem;
}

.logo {
    width: 5rem;
    height: 5rem;
}

.logo-mobile {
    width: 3rem;
    height: 3rem;
}

.carousel-container {
    display: flex;
    align-items: center;
    justify-content: space-around;
    gap: 20px;
    padding-bottom: 0.5rem;
}

.service-card {
    text-align: center;
    width: 150px;
    height: 185px;
    min-height: 11rem;
    margin: 0 20px;
    cursor: pointer;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    border-radius: 10px;
    color: var(--text-color);
    font-weight: bold;
    font-size: 1.2rem;
    background-color: var(--bg-color);
    color: #fff;
}

.service-card:hover {
    filter: brightness(95%);
}

.service-card h4 {
    margin-top: 5px;
    font-size: 1.5rem;
}

.service-card p {
    margin: 5px 0;
    font-size: 1rem;
}

.service-card svg {
    margin-top: 0.9rem;
    font-size: 3rem;
}

.divider {
    width: 70%;
    border: 1px solid #fff;
    margin: 10px 0 5px 0;
}

.nav-arrow {
    background: none;
    border: none;
    font-size: 2rem;
    cursor: pointer;
    position: relative;
    overflow: hidden;
    border-radius: 50%;
    width: 3rem;
    /* for circular button hover */
    height: 3rem;
    /* for circular button hover */
}

.nav-arrow::before {
    content: '';
    position: absolute;
    top: 50%;
    left: 55%;
    width: 0;
    height: 0;
    background-color: rgba(0, 0, 0, 0.1);
    border-radius: 50%;
    transform: translate(-50%, -50%);
    transition: width 0.3s ease, height 0.3s ease;
}

.nav-arrow:hover::before {
    width: 100%;
    height: 100%;
}

.nav-arrow svg {
    height: 1.5rem;
    width: 1.5rem;
}

.service-overview {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    padding: 20px;
}

.service-info {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 20vw;
    min-width: 200px;
    background-color: var(--bg-color);
    color: var(--text-color);
    margin: 0 1rem;
    border-radius: 10px;
}

.service-info .service-icon {
    font-size: 4rem;
    margin-bottom: 1rem;
    margin-top: 1rem;
}

.service-info h3 {
    margin: 0;
    font-size: 2rem;
    margin-bottom: 1rem;
}

.service-info .common-btn {
    margin-bottom: 1rem;
}

.service-details {
    padding-left: 20px;
    width: 60%;
    height: 100%;
}

.service-details h4 {
    font-size: 1.5rem;
    text-align: center;
    margin-bottom: 1rem;
}

/*  */

.actions-reactions-list {
    max-height: 228px;
    overflow-y: auto;
    padding-right: 10px;
}

.action-reaction-card {
    display: flex;
    flex-direction: column;
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 10px;
    margin-bottom: 10px;
    position: relative;
}

.type-section {
    position: absolute;
    top: 10px;
    right: 10px;
    background-color: #f4f4f4;
    padding: 2px 5px;
    border-radius: 3px;
    font-size: 0.8rem;
}

.card-content {
    display: flex;
    align-items: center;
}

.icon-section {
    display: flex;
    align-items: center;
    margin-right: 10px;
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
    color: black;
    font-size: 32px;
}

.info-section {
    flex-grow: 1;
}

/*  */

.service-credentials {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.service-credentials input {
    width: 80%;
    height: 2rem;
    margin: 0.5rem;
    padding: 0.5rem;
    border-radius: 5px;
    border: 1px solid #ccc;
}

.error-message {
    color: red;
    font-size: 1rem;
    margin: 0.5rem;
}

.service-icon {
    font-size: 2rem;
    margin-bottom: 0.5rem;
}

.common-btn {
    background-color: var(--text-color);
    color: var(--bg-color);
    border-radius: 5px;
    border: none;
    width: 60%;
    height: 2rem;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin: 0.5rem;
}

.common-btn:hover {
    filter: brightness(90%);
}
</style>