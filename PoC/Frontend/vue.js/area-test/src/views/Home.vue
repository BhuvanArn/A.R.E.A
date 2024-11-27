<template>
    <NavBar></NavBar>
    <body>
        <div>
            <div class="home-container">
                <input class="input" type="text" placeholder="Sample text">
                <button class="button">Send</button>
                <p class="response">{{ backendResponse }}</p>
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
            backendResponse: 'Response'
        }
    },
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
    position: absolute;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100%;
    width: 20rem;
    left: 50rem;
    bottom: 1rem;
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
}

.response {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 20rem;
    margin-top: 1rem;
    font-family: Arial, Helvetica, sans-serif;
    color: black;
}

</style>
