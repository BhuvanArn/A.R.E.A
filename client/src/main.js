import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from '../axios';

createApp(App).use(store).use(router).use(axios, {
    baseUrl: 'http://localhost:8080',
}).mount('#app')
