<template>
    <body>
        <div class="main-container" :class="{ 'mobile-container': mobile }">
            <div class="main-container-top">
                <h2 class="reset-pwd-title-txt" :class="{ 'mobile-title': mobile }">Reset Password</h2>
                <div class="filler01"></div>
                <img src="@/assets/logo.png" class="logo">
            </div>
            <div class="main-container-bottom">
                <div class="pwd-container">
                    <img src="@/assets/key.png" class="img-style1">
                    <input type="password" class="input-style1" placeholder="Password" v-model="newPassword">
                </div>
                <div class="pwd-container">
                    <img src="@/assets/key.png" class="img-style1">
                    <input type="password" class="input-style1" placeholder="Confirm password" v-model="confirmPassword">
                </div>
                <div v-if="errorMessage" class="error-message">
                    <span>{{ errorMessage }}</span>
                </div>
                <button @click="changePwd" class="send-btn">CONFIRM</button>
            </div>
        </div>
    </body>
</template>

<script>
export default {
    name: 'ResetPwd',
    data() {
        return {
            windowWidth: false,
            mobile: false,
            email: '',
            newPassword: '',
            confirmPassword: '',
            errorMessage: ''
        }
    },
    mounted() {
        window.addEventListener('resize', this.checkScreen);
        this.checkScreen();
    },
    methods: {
        checkScreen() {
            this.windowWidth = window.innerWidth;
            if (this.windowWidth <= 960) {
                this.mobile = true;
            } else {
                this.mobile = false;
            }
        },

        async changePwd() {
            if (this.newPassword !== this.confirmPassword) {
                this.errorMessage = 'Passwords do not match';
                return;
            }
            this.errorMessage = '';
            // Send the request to the backend to change the password
        }
    }
}
</script>

<style>
body {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100vh;
    margin: 0;
    padding: 0;
    background-color: #efefef;
}

.main-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    width: 40rem;
    height: 25rem;
    background-color: transparent;
}

.mobile-container {
    width: 25rem;
    height: 25rem;
}

.main-container-bottom {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 80%;
    background-color: #efefef;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    gap: 0.5rem;
}

.main-container-top {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 30%;
    background-color: #bcc1ba;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
    border-bottom-color: black;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    border: 1px solid transparent;
    border-bottom-color: #000000;
}

.reset-pwd-title-txt {
    font-size: 3rem;
    color: #313030;
    margin: 0;
    padding: 0;
    margin-left: 1.5rem;
    margin-right: 1rem;
    font-family: 'inter', sans-serif;
    font-weight: 400;
    white-space: nowrap;
}

.mobile-title {
    font-size: 2.3rem;
}

.logo {
    width: 5rem;
    height: 5rem;
    margin-left: 1rem;
    margin-right: 1.5rem;
}

.filler01 {
    width: 100%;
    height: 100%;
    background-color: transparent;
}

.filler02 {
    width: 100%;
    height: 1rem;
    background-color: transparent;
}

.email-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: transparent;
    height: 3rem;
    width: 23rem;
}

.pwd-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    background-color: transparent;
    height: 3rem;
    width: 23rem;
    margin-bottom: 1rem;
}

.img-style1 {
    width: 1.5rem;
    height: 1.5rem;
    margin-left: 1rem;
    margin-right: 1rem;
}

.img-style2 {
    width: 2.1rem;
    height: 2.1rem;
    margin-left: 1rem;
    margin-right: 1rem;
}

.input-style1 {
    width: 17rem;
    height: 2rem;
    background-color: transparent;
    border: none;
    border-bottom: 2px solid #bcc1ba;
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin-right: 1rem;
    outline: none;
    transition: border-bottom 0.35s ease;
}

.link {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #28728B;
    cursor: pointer;
}

.send-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #28728B;
    border-radius: 5px;
    border: none;
    width: 8rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 1rem;
    margin-bottom: 1rem;
}

.send-btn:hover {
    background-color: #3a9cb1;
}

.send-btn:active {
    background-color: #2e7f8f;
}

.or-filler {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 23rem;
    height: 2rem;
    background-color: transparent;
}

.or-hr {
    width: 10rem;
    background-color: #bcc1ba;
}

.or-txt {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin-left: 0.5rem;
    margin-right: 0.5rem;
}

.oauth-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 20rem;
    height: 4rem;
    background-color: transparent;
    gap: 1rem;
    margin-bottom: 1rem;
}

.oauth-ind-container-fk {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(65, 153, 230);
    cursor: pointer;
}

.oauth-ind-container-fk:hover {
    background-color: rgb(50, 122, 184);
}

.oauth-ind-container-fk:active {
    background-color: rgb(35, 91, 138);
}

.oauth-ind-container-gg {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(255, 255, 255);
    cursor: pointer;
}

.oauth-ind-container-mc {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(255, 255, 255);
    cursor: pointer;
}

.oauth-ind-container-gg:hover, .oauth-ind-container-mc:hover {
    background-color: rgb(240, 240, 240);
}

.oauth-ind-container-gg:active, .oauth-ind-container-mc:active {
    background-color: rgb(225, 225, 225);
}

.oauth-ind-container-dc {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 4rem;
    height: 3rem;
    border-radius:5px;
    background-color: rgb(55, 133, 222);
    cursor: pointer;
}

.oauth-ind-container-dc:hover {
    background-color: rgb(40, 102, 170);
}

.oauth-ind-container-dc:active {
    background-color: rgb(25, 65, 107);
}

.txt-link {
    font-family: 'inter', sans-serif;
    font-weight: 300;
    cursor: pointer;
    margin: 0;
    padding: 0;
}

.rst-pwd-instruction-container {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin: 0;
    padding: 0;
    margin-left: 1rem;
    margin-right: 1rem;
    text-align: center;
}

.error-message {
    color: red;
    font-family: 'inter', sans-serif;
    font-size: medium;
    margin-bottom: -1rem;
}

</style>