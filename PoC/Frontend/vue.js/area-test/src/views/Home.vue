<template>
    <NavBar></NavBar>
    <body>
        <div>
            <div class="home-container">
                <h1 class="title">Test input</h1>
                <input class="input" type="text" placeholder="Sample text" v-model="message">
                <button class="button" @click="sendRequest">Send</button>
                <p v-if="status === 'success'" class="success">Request succeeded!</p>
                <p v-if="status === 'error'" class="error">Request failed!</p>
            </div>
        </div>
    </body>
</template>

<script>
import NavBar from '../components/NavBar.vue';

export default {
    name: 'Home',
    components: {
        NavBar
    },
    data() {
        return {
            message: '',
            status: ''
        }
    },
    methods: {
        async sendRequest() {
            try {
                const res = await fetch('http://localhost:8000/', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ msg: this.message })
                });
                this.status = res.ok ? 'success' : 'error';
            } catch (err) {
                console.error('Error:', err);
                this.status = 'error';
            } finally {
                setTimeout(() => {
                    this.status = '';
                }, 3000);
            }
        }
    }
};
</script>

<style>

body {
	background-color: #f5f5f5;
	width: 100%;
	background-repeat: no-repeat;
	margin: 0;
	height: 100%;
	display: flex;
	flex-direction: column;
}

.home-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
}

.title {
    display: flex;
    justify-content: center;
    font-family: Arial, Helvetica, sans-serif;
    color: black;
}

.input {
    width: 50%;
    height: 1.5rem;
    border: transparent;
    border-radius: 10px;
}

.button {
    width: 30%;
    height: 1.5rem;
    border: transparent;
    border-radius: 10px;
    background-color: #f9f9f9;
    cursor: pointer;
    margin-top: 1rem;
    font-family: Arial, Helvetica, sans-serif;
    color: black;
    transition: background-color 0.5s ease;
}

.button:hover {
    animation: rainbow 1s infinite;
}

@keyframes rainbow {
    0% { background-color: #ff0000; }
    14% { background-color: #ff7f00; }
    28% { background-color: #ffff00; }
    42% { background-color: #00ff00; }
    57% { background-color: #0000ff; }
    71% { background-color: #4b0082; }
    85% { background-color: #8b00ff; }
    100% { background-color: #ff0000; }
}

.success {
    color: green;
    margin-top: 1rem;
}

.error {
    color: red;
    margin-top: 1rem;
}

</style>
