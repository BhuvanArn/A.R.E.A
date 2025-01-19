<template>
    <div class="table-container" :class="{ 'mobile-table': mobile }">
        <table class="areas-table">
            <thead>
                <tr class="table-header">
                    <th>Status</th>
                    <th>Service</th>
                    <th>Area's Name</th>
                    <th>Number of reactions</th>
                    <th>Creation Date</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <template v-for="(area, index) in areas" :key="index">
                    <tr class="table-row">
                        <td class="status-column">
                            <svg v-if="area.state === 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" fill="#1c7a20">
                                <path d="M256 48a208 208 0 1 1 0 416 208 208 0 1 1 0-416zm0 464A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-111 111-47-47c-9.4-9.4-24.6-9.4-33.9 0s-9.4 24.6 0 33.9l64 64c9.4 9.4 24.6 9.4 33.9 0L369 209z"/>
                            </svg>
                            <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" fill="#777">
                                <path d="M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm192-96l128 0c17.7 0 32 14.3 32 32l0 128c0 17.7-14.3 32-32 32l-128 0c-17.7 0-32-14.3-32-32l0-128c0-17.7 14.3-32 32-32z"/>
                            </svg>
                        </td>
                        <td class="icon-column">
                            <Iconify :icon="getServiceIcon(area.serviceName)" class="service-icon" />
                        </td>
                        <td class="name-column">{{ area.displayName }}</td>
                        <td class="number-column">1</td>
                        <td class="date-column">{{ formatDate(area.createdDate) }}</td>
                        <td class="actions-column">
                            <button class="action-btn update-btn" @click="updateArea(area)">
                                <div class="icon-square">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z"/></svg>
                                </div>
                            </button>
                            <button class="action-btn delete-btn" @click="deleteArea(area)">
                                <div class="icon-square">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512"><path d="M170.5 51.6L151.5 80l145 0-19-28.4c-1.5-2.2-4-3.6-6.7-3.6l-93.7 0c-2.7 0-5.2 1.3-6.7 3.6zm147-26.6L354.2 80 368 80l48 0 8 0c13.3 0 24 10.7 24 24s-10.7 24-24 24l-8 0 0 304c0 44.2-35.8 80-80 80l-224 0c-44.2 0-80-35.8-80-80l0-304-8 0c-13.3 0-24-10.7-24-24S10.7 80 24 80l8 0 48 0 13.8 0 36.7-55.1C140.9 9.4 158.4 0 177.1 0l93.7 0c18.7 0 36.2 9.4 46.6 24.9zM80 128l0 304c0 17.7 14.3 32 32 32l224 0c17.7 0 32-14.3 32-32l0-304L80 128zm80 64l0 208c0 8.8-7.2 16-16 16s-16-7.2-16-16l0-208c0-8.8 7.2-16 16-16s16 7.2 16 16zm80 0l0 208c0 8.8-7.2 16-16 16s-16-7.2-16-16l0-208c0-8.8 7.2-16 16-16s16 7.2 16 16zm80 0l0 208c0 8.8-7.2 16-16 16s-16-7.2-16-16l0-208c0-8.8 7.2-16 16-16s16 7.2 16 16z"/></svg>
                                </div>
                            </button>
                        </td>
                    </tr>
                    <tr class="spacer-row">
                        <td colspan="100"></td>
                    </tr>
                </template>
                <tr class="create-row" @click="createArea">
                    <td colspan="100">
                        <div class="create-content">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" fill="#000000">
                                <path d="M432 256c0 13.3-10.7 24-24 24H272v136c0 13.3-10.7 24-24 24s-24-10.7-24-24V280H88c-13.3 0-24-10.7-24-24s10.7-24 24-24h136V96c0-13.3 10.7-24 24-24s24 10.7 24 24v136h136c13.3 0 24 10.7 24 24z"/>
                            </svg>
                            Create your own AREA(s) !
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <CreateAreaModal v-if="showCreateModal" @close="showCreateModal = false" />
</template>

<script>
import { Icon } from "@iconify/vue";
import CreateAreaModal from "./CreateAreaModal.vue";
import { getCookie, removeCookie, setCookie } from '@/utils/cookies';

export default {
    name: "TableAreas",
    components: {
        Iconify: Icon,
        CreateAreaModal,
    },
    data() {
        return {
            showCreateModal: false,
            areas: [],
            mobile: false,
        };
    },
    methods: {
        getServiceIcon(name) {
            if (!name) {
                return "mdi:help-circle";
            }
            const lowerCased = name.toLowerCase();
            return `mdi:${lowerCased}`;
        },
        formatDate(dateString) {
            const options = { year: 'numeric', month: 'long', day: 'numeric' };
            return new Date(dateString).toLocaleDateString(undefined, options);
        },
        updateArea(area) {
            console.log("Update area:", area);

            // Redirect to update page/modal
        },
        async deleteArea(area) {
            //debug
            console.log("Deleting area:", area.actionId);

            try {
                const body = { "ActionId": area.actionId };

                const token = getCookie('token');

                const res = await this.$axios.delete(`/area/delete_areas`, {
                    data: body,
                    headers: {
                        'X-User-Token': token,
                        'Content-Type': 'application/json'
                    }
                });

                this.$router.go();
            } catch (error) {
                console.error("Error deleting area:", error);
            }
        },

        async getServiceNameWithId(serviceId) {
            try {
                const token = getCookie('token');
                const res = await this.$axios.get(`/area/services/false`, {
                    headers: {
                        'X-User-Token': token
                    }
                });

                const service = res.data.find(service => service.id === serviceId);
                return service.name;
            } catch (error) {
                console.error("Error getting service name:", error);
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

        async getAreas() {
            try {
                const token = getCookie('token');
                const res = await this.$axios.get("/area/services/true", {
                    headers: {
                        'X-User-Token': token
                    }
                });

                console.log("Areas:", res);

                this.areas = res.data;

                this.areas.forEach(async area => {
                    const serviceName = await this.getServiceNameWithId(area.serviceId);
                    area.serviceName = serviceName;
                });
            } catch (error) {
                console.error("Error getting areas:", error);
            }
        },
        createArea() {
            this.showCreateModal = true;
        }
    },
    async mounted() {
        await this.getAreas();
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();

    }
};
</script>

<style scoped>

.table-container {
    width: 100%;
    overflow-x: auto;
    background-color: #efefef;
    margin: 0 0.5rem;
}

.areas-table {
    width: 100%;
    border-collapse: collapse;
    margin: 20px 0;
    font-size: 1rem;
    text-align: left;
}

.areas-table th, .areas-table td {
    padding: 12px 15px;
}

.service-icon {
    font-size: 30px;
}

.action-btn {
    background: none;
    border: none;
    cursor: pointer;
    margin-right: 10px;
    display: flex;
    align-items: center;
}

.icon-square {
    width: 30px;
    height: 29px;
    background-color: #f4f4f4;
    border: 2px solid #e8e7e4;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;

    svg {
        width: 16px;
        height: 16px;
    }
}

.icon-square:hover {
    background-color: #e8e7e4;
    border-color: #c4c4c4;
    /* animate the hover effect */
    transition: background-color 0.1s, border-color 0.1s;
}

.icon-square:active {
    background-color: #c4c4c4;
    border-color: #a4a4a4;
    /* animate the active effect */
    transition: background-color 0.1s, border-color 0.1s;
}

/* Each column */

.status-column {
    width: 6%;

    padding-left: 25px!important;

    svg {
        width: 2.1rem;
    }
}

.icon-column {
    width: 6%;
    padding-left: 30px!important;
}

.date-column {
    width: 15%;
}

.name-column {
    width: 50%;
}

.number-column {
    width: 11%;
}

.actions-column {
    display: flex;
    align-items: center;
    padding: 17px 15px!important;
    width: 100%;
}

/* header of table */

.table-header {
    color: #212529;

    th {
        border-top: none;
        border-bottom: none;
        font-size: 1.1rem;
    }
}

/* rows of table */

.table-row {
    border-radius: 7px;
    overflow: hidden;
    transition: .3s all ease;

    td:first-child {
        border-top-left-radius: 7px;
        border-bottom-left-radius: 7px;
    }

    td {
        background-color: #fff;
        border: none;
        color: #101010;
        font-weight: 450;
    }

    td:last-child {
        border-top-right-radius: 7px;
        border-bottom-right-radius: 7px;
    }
}

/* spacer row */

.spacer-row td {
    height: 10px;
    background-color: transparent;
    border: none;
    padding: 0;
}

/* create row */

.create-row {
    cursor: pointer;
    background-color: #f4f4f4;
    text-align: center;
    transition: background-color 0.3s;
}

.create-row:hover {
    background-color: #e8e7e4;
}

.create-content {
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 10px 0;
    font-weight: bold;
    color: #000;

    svg {
        margin-right: 10px;
        width: 20px;
        height: 20px;
    }
}

</style>
