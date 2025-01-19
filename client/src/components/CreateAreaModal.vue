<template>
    <div class="modal-overlay" @click.self="closeModal">
        <div class="modal">
            <div v-if="!mobile" class="modal-header">
                <h2 class="modal-title">Create AREA</h2>
                <img src="@/assets/logo.png" class="logo">
            </div>
            <div v-if="mobile" class="modal-header">
                <h2 class="modal-title-mobile">Create AREA</h2>
                <img src="@/assets/logo.png" class="logo-mobile">
            </div>
            <div class="modal-body">
                <div v-if="step === 1" class="puzzle-container">
                    <div class="puzzle-piece left-piece" :class="{'puzzle-piece-mobile':mobile}" :style="actionStyle" @click="selectPart('action')">
                        <div v-if="selectedAction" class="each-piece">
                            <Iconify :icon="getServiceIcon(selectedActionService.name)" class="service-icon-large" />
                            <p style="font-style: italic;">{{ selectedAction.name.slice(0, 1).toUpperCase() + selectedAction.name.slice(1) }}</p>
                            <p>{{ selectedAction.description }}</p>
                            <Iconify v-if="selectedAction" icon="mdi:trash-can-outline" class="trash-icon" @click.stop="deleteSelectedPart('action')" />
                        </div>
                        <p v-else>Select the Action</p>
                    </div>
                    <div class="puzzle-piece right-piece" :class="{'puzzle-piece-mobile':mobile}" :style="reactionStyle" @click="selectPart('reaction')">
                        <div v-if="selectedReaction" class="each-piece">
                            <Iconify :icon="getServiceIcon(selectedReactionService.name)" class="service-icon-large" />
                            <p style="font-style: italic;">{{ selectedReaction.name.slice(0, 1).toUpperCase() + selectedReaction.name.slice(1) }}</p>
                            <p>{{ selectedReaction.description }}</p>
                            <Iconify v-if="selectedReaction" icon="mdi:trash-can-outline" class="trash-icon" @click.stop="deleteSelectedPart('reaction')" />
                        </div>
                        <p v-else>Select the Reaction</p>
                    </div>
                </div>
                <div v-if="step === 2 || step === 4" class="carousel-container">
                    <div v-if="subscribedServices.length === 0" class="no-services">
                        No service available
                    </div>
                    <div v-else-if="subscribedServices.length <= 3" class="carousel-container">
                        <div
                            class="service-card"
                            v-for="(service, idx) in subscribedServices"
                            :key="idx"
                            @click="viewService(service)"
                            :style="{ '--bg-color': getBrandColor(service.name) }">
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
                            <svg fill="#000000" height="16px" width="16px" viewBox="-33 -33 396 396" transform="rotate(180)">
                                <path d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606C255,161.018,253.42,157.202,250.606,154.389z" />
                            </svg>
                        </button>
                        <div
                            class="service-card"
                            v-for="(service, idx) in visibleServices"
                            :key="idx"
                            @click="viewService(service)"
                            :style="{ '--bg-color': getBrandColor(service.name) }">
                            <Iconify :icon="getServiceIcon(service.name)" />
                            <h4>{{ service.name.slice(0, 1).toUpperCase() + service.name.slice(1) }}</h4>
                            <hr class="divider" />
                            <p>{{ service.actions.length }} Action{{ service.actions.length > 1 ? 's' : '' }}</p>
                            <p>{{ service.reactions.length }} Reaction{{ service.reactions.length > 1 ? 's' : '' }}</p>
                        </div>
                        <!-- right arrow SVG -->
                        <button class="nav-arrow" @click="nextService">
                            <svg fill="#000000" height="16px" width="16px" viewBox="-33 -33 396 396">
                            <path d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606C255,161.018,253.42,157.202,250.606,154.389z" />
                            </svg>
                        </button>
                    </div>
                </div>
                <div v-if="step === 3 || step === 5" class="service-overview">
                    <div class="service-info" :style="{ '--bg-color': getBrandColor(selectedService.name) }">
                        <Iconify :icon="getServiceIcon(selectedService.name)" class="service-icon" />
                        <h3>{{ selectedService.name.slice(0, 1).toUpperCase() + selectedService.name.slice(1) }}</h3>
                    </div>
                    <div class="service-details">
                        <div v-if="step === 3">
                            <h4>Supported actions</h4>
                            <div class="actions-reactions-list">
                                <div v-for="action in selectedService.actions" :key="action.name" class="action-reaction-card" @click="toggleDropdown(action)">
                                    <div class="type-section">
                                        <small>Action</small>
                                    </div>
                                    <div class="card-content">
                                        <div class="icon-section">
                                            <div class="icon-square">
                                                <Iconify :icon="getServiceIcon(selectedService.name)" class="service-icon-small" />
                                            </div>
                                        </div>
                                        <div class="info-section">
                                            <strong>{{ truncateText(action.name, 30) }}</strong>
                                            <p>{{ truncateText(action.description, 50) }}</p>
                                        </div>
                                    </div>
                                    <div v-if="dropdownAction === action.name" class="dropdown-menu" @click.stop>
                                        <strong v-if="action.inputs.length > 0">Inputs</strong>
                                        <div v-for="input in action.inputs" :key="input.name">
                                            <input :id="input.name" v-model="actionInputs[input.name]" type="text" :placeholder="input.name" />
                                        </div>
                                        <div style="display: flex; justify-content: center;">
                                            <button class="common-btn" @click="selectAction(action)">Select</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div v-if="step === 5">
                            <h4>Supported reactions</h4>
                            <div class="actions-reactions-list">
                                <div v-for="reaction in selectedService.reactions" :key="reaction.name" class="action-reaction-card" @click="toggleDropdown(reaction)">
                                    <div class="type-section">
                                        <small>Reaction</small>
                                    </div>
                                    <div class="card-content">
                                        <div class="icon-section">
                                            <div class="icon-square">
                                                <Iconify :icon="getServiceIcon(selectedService.name)" class="service-icon-small" />
                                            </div>
                                        </div>
                                        <div class="info-section">
                                            <strong>{{ truncateText(reaction.name, 30) }}</strong>
                                            <p>{{ truncateText(reaction.description, 50) }}</p>
                                        </div>
                                    </div>
                                    <div v-if="dropdownReaction === reaction.name" class="dropdown-menu" @click.stop>
                                        <div v-for="input in reaction.inputs" :key="input.name">
                                            <label :for="input.name">{{ input.name }}</label>
                                            <input :id="input.name" v-model="reactionInputs[input.name]" type="text" />
                                        </div>
                                        <div style="display: flex; justify-content: center;">
                                            <button class="common-btn" @click="selectReaction(reaction)">Select</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer-1" v-if="step > 1">
                <button @click="prevStep">Back</button>
            </div>
            <div class="modal-footer-2" v-if="selectedAction && selectedReaction && step == 1">
                <input v-model="areaName" type="text" placeholder="Name of your area" />
                <button @click="createArea" :disabled="!areaName">Confirm</button>
            </div>
        </div>
    </div>
</template>

<script>
import { Icon } from "@iconify/vue";
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

export default {
    name: "CreateAreaModal",
    components: {
        Iconify: Icon
    },
    data() {
        return {
            step: 1,
            indexStart: 0,
            subscribedServices: [],

            selectedActionService: null,
            selectedAction: null,

            actionInputs: {},

            selectedReactionService: null,
            selectedReaction: null,

            reactionInputs: {},

            dropdownAction: null,
            dropdownReaction: null,

            hasSelectedAction: false,
            hasSelectedReaction: false,

            mobile: false,

            areaName: "",
        };
    },
    computed: {
        actionStyle() {
            return this.selectedActionService
                ? { backgroundColor: this.getBrandColor(this.selectedActionService.name), color: this.getComplementaryColor(this.getBrandColor(this.selectedActionService.name)) }
                : { color: '#777' };
        },
        reactionStyle() {
            return this.selectedReactionService
                ? { backgroundColor: this.getBrandColor(this.selectedReactionService.name), color: this.getComplementaryColor(this.getBrandColor(this.selectedReactionService.name)) }
                : { color: '#777' };
        },
        visibleServices() {
            if (this.subscribedServices.length <= 3) {
                return this.subscribedServices;
            }
            const result = [];
            for (let i = 0; i < 3; i++) {
                const idx = (this.indexStart + i) % this.subscribedServices.length;
                result.push(this.subscribedServices[idx]);
            }
            return result;
        }
    },
    methods: {
        closeModal() {
            this.$emit('close');
        },
        nextService() {
            this.indexStart = (this.indexStart + 1) % this.subscribedServices.length;
        },
        prevService() {
            this.indexStart = (this.indexStart - 1 + this.subscribedServices.length) % this.subscribedServices.length;
        },
        async fetchSubscribedServices() {
            try {
                const token = getCookie('token');
                const registeredService = await this.$axios.get(`/area/services/false`, {
                    headers: {
                        'X-User-Token': token,
                    },
                });

                const allServices = await this.$axios.get("/about.json");

                // Filter out services that are not registered
                this.subscribedServices = allServices.data.server.services.filter(service => {
                    return registeredService.data.some(registered => registered.name === service.name);
                });
            } catch (error) {
                console.error(error);
            }
        },
        selectPart(part) {
            if (part === 'action') {
                this.step = 2;
            } else if (part === 'reaction') {
                this.step = 4;
            }
        },

        viewService(service) {
            this.selectedService = service;
            if (this.step === 2) {
                this.selectedActionService = service;
                this.step = 3;
            } else if (this.step === 4) {
                this.selectedReactionService = service;
                this.step = 5;
            }
        },
        toggleDropdown(item) {
            if (item.name === this.dropdownAction) {
                this.dropdownAction = null;
            } else if (item.name === this.dropdownReaction) {
                this.dropdownReaction = null;
            } else {
                this.dropdownAction = item.name;
                this.dropdownReaction = item.name;
            }
        },

        selectAction(action) {
            this.selectedAction = action;
            this.step = 1;
            this.hasSelectedAction = true;
        },
        selectReaction(reaction) {
            this.selectedReaction = reaction;
            this.step = 1;
            this.hasSelectedReaction = true;
        },
        prevStep() {
            if (this.step === 5) {
                this.step = 4;
            } else if (this.step === 3) {
                this.step = 2;
            } else if (this.step === 2) {
                this.selectedService = null;
                if (!this.hasSelectedAction) {
                    this.selectedActionService = null;
                }
                this.step = 1;
            } else if (this.step === 4) {
                if (!this.hasSelectedReaction) {
                    this.selectedReactionService = null;
                }
                this.selectedService = null;
                this.step = 1;
            }
        },

        async getIdOfService(serviceName) {
            try {
                const res = await this.$axios.get(`/area/services/false`, {
                    headers: {
                        'X-User-Token': getCookie('token'),
                    },
                });

                const service = res.data.find(service => service.name === serviceName);

                return service.id;
            } catch (error) {
                console.error(error);
                return null;
            }
        },

        async createArea() {
            try {
                const token = getCookie('token');

                console.log(this.selectedActionService, this.selectedAction, this.actionInputs);
                console.log(this.selectedReactionService, this.selectedReaction, this.reactionInputs);

                const actionServiceId = await this.getIdOfService(this.selectedActionService.name);
                const reactionServiceId = await this.getIdOfService(this.selectedReactionService.name);

                const resAction = await this.$axios.post(`/area/addactions`, {
                    "ServiceId": actionServiceId,
                    "Name": this.selectedAction.name,
                    "DisplayName": this.areaName,
                    "TriggerConfig": JSON.stringify(this.actionInputs),
                }, {
                    headers: {
                        'X-User-Token': token,
                    },
                });

                const resReaction = await this.$axios.post(`/area/addreactions`, {
                    "ServiceId": reactionServiceId,
                    "ActionId": resAction.data,
                    "Name": this.selectedReaction.name,
                    "ExecutionConfig": JSON.stringify(this.reactionInputs),
                }, {
                    headers: {
                        'X-User-Token': token,
                    },
                });

                this.$router.go();
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

        getServiceIcon(name) {
            const lowerCased = name.toLowerCase();

            if (lowerCased === 'test') {
                return 'mdi:robot';
            }
            return `mdi:${lowerCased}`;
        },

        truncateText(text, maxLength) {
            if (text.length > maxLength) {
                return text.slice(0, maxLength) + '...';
            }
            return text;
        },

        deleteSelectedPart(part) {
            if (part === 'action') {
                this.selectedAction = null;
                this.selectedActionService = null;
                this.actionInputs = {};
                this.hasSelectedAction = false;
            } else if (part === 'reaction') {
                this.selectedReaction = null;
                this.selectedReactionService = null;
                this.reactionInputs = {};
                this.hasSelectedReaction = false;
            }
        },

        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },
    },
    async mounted() {
        await this.fetchSubscribedServices();
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
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
    height: 20%;
    border-top-left-radius: 7px;
    border-top-right-radius: 7px;
    margin-bottom: 1rem;

    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 20%;
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
    margin: 0;
    font-family: 'inter', sans-serif;
    font-weight: 400;
    font-size: 2rem;
    color: #313030;
}

.modal-body {
    padding: 20px;
}

.modal-footer-1 {
    display: flex;
    justify-content: center;
    padding: 20px;
    border-top: 1px solid #ccc;

    button {
        background-color: #28728B;
        color: #efefef;
        border-radius: 5px;
        border: none;
        width: 8rem;
        height: auto;
        font-size: 1.1rem;
        font-weight: 600;
        cursor: pointer;
    }

    button:hover {
        background-color: #3a9cb1;
    }

    button:active {
        background-color: #2e7f8f;
    }
}

.modal-footer-2 {
    display: flex;
    justify-content: space-between;
    padding: 20px;
    border-top: 1px solid #ccc;

    input {
        width: 25%;
        padding: 0.75rem 0.3rem;
        font-size: 1.1rem;
        border-radius: 5px;
        border: 1px solid #ccc;
    }

    button {
        background-color: #28728B;
        color: #efefef;
        border-radius: 5px;
        border: none;
        width: 8rem;
        height: 2rem;
        font-size: 1.1rem;
        font-weight: 600;
        cursor: pointer;
    }

    button:hover {
        background-color: #3a9cb1;
    }

    button:active {
        background-color: #2e7f8f;
    }

    button:disabled {
        background-color: #ccc;
        cursor: not-allowed;
    }

    button {
        height: auto;
    }
}


.puzzle-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 250px;
    border: none;
}

.puzzle-piece {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 200px;
    background-color: #f4f4f4;
    cursor: pointer;
    position: relative;
    border-right: 2px dotted #ccc;
}

.left-piece {
    border-top-left-radius: 15px;
    border-bottom-left-radius: 15px;
    border-left: 2px solid #ccc;
    border-top: 2px solid #ccc;
    border-bottom: 2px solid #ccc;
}

.right-piece {
    border-top-right-radius: 15px;
    border-bottom-right-radius: 15px;
    border-right: 2px solid #ccc;
    border-top: 2px solid #ccc;
    border-bottom: 2px solid #ccc;
}

.each-piece {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
}

.service-icon-large {
    font-size: 40px;
    margin-bottom: 10px;
}

.puzzle-piece p {
    margin: 0;
    font-size: 1.2rem;
}

.puzzle-piece-mobile p {
    margin: 0;
    font-size: 1rem;
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
    margin-bottom: 10px;
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
    margin-right: 10px;
    flex-shrink: 0;
}

.service-icon-small {
    color: black;
    font-size: 20px;
}

.service-name {
    font-size: 1.25rem;
    font-weight: 600;
    margin-left: 10px;
    flex-grow: 1;
}

.action-name, .reaction-name {
    font-size: 1.25rem;
    font-weight: bold;
}

.action-description, .reaction-description {
    font-size: 1rem;
    color: #555;
}

input {
    width: 100%;
    padding: 10px;
    margin: 5px 0;
    border-radius: 5px;
    border: 1px solid #ccc;
}

/* Carousel */

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
    height: calc(30vh - 2rem);
    min-height: 11rem;
    margin: 0 20px;
    cursor: pointer;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    border-radius: 10px;
    color: #fff;
    font-weight: bold;
    font-size: 1.2rem;
    background-color: var(--bg-color);
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
    width: 3rem; /* for circular button hover */
    height: 3rem; /* for circular button hover */
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

.logo-mobile {
    width: 50px;
    height: 50px;
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
    color: #fff;
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

.dropdown-menu {
    background-color: #fff;
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 10px;
    margin-top: 10px;
}

.dropdown-menu label {
    display: block;
    margin-bottom: 5px;
}

.dropdown-menu input {
    width: 100%;
    padding: 5px;
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

.common-btn {
    background-color: #28728B;
    color: #efefef;
    border-radius: 5px;
    border: none;
    width: 30%;
    height: 2rem;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin: 0.5rem;
}

.common-btn:hover {
    filter: brightness(90%);
}

/* Trash icon */

.trash-icon {
    position: absolute;
    top: 10px;
    right: 10px;
    font-size: 1.5rem;
    color: red;
    cursor: pointer;
    display: none;
}

.puzzle-piece:hover .trash-icon {
    display: block;
}

.txt-mobile {
    font-size: 0.5rem;
}

</style>
