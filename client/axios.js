import axios from 'axios'

class AxiosOptions {
    baseUrl
    token
}

export default {
    install: (app, options) => {
        app.config.globalProperties.$axios = axios.create({
            baseURL: options.baseUrl,
            headers: {
                Authorization: options.token ? `Bearer ${options.token}` : '',
            }
        })
    }
}