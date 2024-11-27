<template>
    <div>
        <NavBar></NavBar>
        <div class="response-table-container">
            <button class="refresh-button" @click="fetchResponses">Refresh</button>
            <table class="response-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Message</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="response in responses" :key="response.id">
                        <td>{{ response.id }}</td>
                        <td>{{ response.name }}</td>
                        <td>{{ response.message }}</td>
                        <td><button @click="deleteResponse(response.id)">Delete</button></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import NavBar from '../components/NavBar.vue';

export default {
    name: 'ResponseTable',
    components: {
        NavBar
    },
    data() {
        return {
            responses: []
        };
    },
    methods: {
        async fetchResponses() {
            try {
                const res = await fetch('http://localhost:8000/users', {
                    method: 'GET'
                });
                this.responses = await res.json();
            } catch (err) {
                console.error('Error fetching responses:', err);
            }
        },
        async deleteResponse(id) {
            try {
                await fetch(`http://localhost:8000/users/${id}`, {
                    method: 'DELETE'
                });
            } catch (err) {
                console.error('Error deleting response:', err);
            }
        }
    },
    mounted() {
        this.fetchResponses();
    }
};
</script>

<style scoped>

.response-table-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
}

.refresh-button {
    margin-bottom: 20px;
}

.response-table {
    width: 80%;
    border-collapse: collapse;
}

.response-table th, .response-table td {
    border: 1px solid #ddd;
    padding: 8px;
}

.response-table th {
    background-color: #f2f2f2;
}

</style>