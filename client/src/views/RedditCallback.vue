<template>
</template>

<script>

export default {
    name: 'RedditCallback',
    props: {
        code: String,
    },
    data() {
        return {
        };
    },
    methods: {

        async getRedditToken() {
            try {

                const token = localStorage.getItem('token');

                const response = await this.$axios.post('/oauth/reddit/access_token', {
                    code: this.code,
                    redirect_url: 'http://localhost:8081/oauth/reddit/callback',
                });

                console.log(response);

                const subscribe = await this.$axios.post('/area/subscribe_service', {
                    name: 'reddit',
                    auth: {
                        "token": response.data.access_token,
                    }
                },
                    {
                        headers: {
                            'X-User-Token': token,
                        },
                    });

                if (window.opener) {
                    window.opener.location.reload();
                }

                window.close();
            } catch (error) {
                console.log(error);
                setTimeout(() => {
                    window.close();
                }, 3000);
            }
        },
    },
    async mounted() {

        await this.getRedditToken();
    },
};
</script>