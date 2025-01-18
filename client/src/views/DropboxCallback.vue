<template>
</template>

<script>

export default {
  name: 'DropboxCallback',
  props: {
    code: String,
  },
  data() {
    return {
    };
  },
  methods: {

    async getDropboxToken() {
      try {

        const token = localStorage.getItem('token');

        const response = await this.$axios.post('/oauth/dropbox/access_token', {
          code: this.code,
          redirect_url: 'http://localhost:8081/oauth/dropbox/callback',
        });

        console.log(response);

        const subscribe = await this.$axios.post('/area/subscribe_service', {
            name: 'dropbox',
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

    await this.getDropboxToken();
  },
};
</script>