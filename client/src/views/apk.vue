<template>
    <div>
        <div>You are downloading the APK file...</div>
        <a :href="apkUrl" download>Click here if the download does not start automatically.</a>
    </div>
</template>

<script>

export default {
    name: 'AreaApk',
    data() {
        return {
            apkUrl: '/shared/client.apk',
        };
    },
    mounted() {
        // Redirect to APK file
        window.location.href = this.apkUrl;
    },
};

</script>
