<template>
        <div class="account-section-container">
            <div class="main-container-top">
                <h2 class="account-title-txt">Profile</h2>
                <div class="filler01"></div>
                <img src="@/assets/logo.png" class="logo-account" alt="logo">
            </div>
            <div v-if="isAreaAccount" class="main-container-bottom">
                <div class="filler01"></div>
                <span class="account-avatar-container">
                    <!-- <img :src="getAvatarSrc" class="avatar-img"> -->
                    <h2 class="user-avatar-ini">{{ userAvatar }}</h2>
                </span>
                <h2 class="email-txt"> {{ userEmail }} </h2>
                <div class="filler02"></div>
                <hr class="hr-style01">
                <div class="filler01"></div>
                <div class="section-container">
                    <h2 class="profile-title-txt-style">Username</h2>
                    <div class="filler03"></div>
                    <div class="user-name-container">
                        <img src="@/assets/user.png" class="img-style1" alt="username">
                        <input type="text" class="input-style1" placeholder="Username" v-model="userName">
                    </div>
                </div>
                <div class="filler03"></div>
                <div class="section-container">
                    <h2 class="profile-title-txt-style">Password</h2>
                    <div class="filler03"></div>
                    <div class="user-pwd-container">
                        <img src="@/assets/key.png" class="img-style1" alt="password">
                        <button v-show="!changePwdMenu" @click="activateChangePwdMenu" class="change-pwd-btn">Change</button>
                        <input v-show="changePwdMenu" type="password" class="input-pwd-style1" placeholder="Old password" v-model="oldPwd">
                    </div>
                    <div v-show="changePwdMenu" class="filler02"></div>
                    <div v-show="changePwdMenu" class="user-pwd-container">
                        <div class="filler04"></div>
                        <input type="password" class="input-pwd-style1" placeholder="New password" v-model="newPwd">
                    </div>
                    <div v-show="changePwdMenu" class="filler02"></div>
                    <div v-show="changePwdMenu" class="user-pwd-container">
                        <div class="filler04"></div>
                        <input type="password" class="input-pwd-style1" placeholder="Confirm new password" v-model="confirmNewPwd">
                    </div>
                    <div v-show="errorMessage" class="filler02"></div>
                    <div v-if="errorMessage" class="error-message-section">
                        <div v-if="errorMessage" class="error-message">
                            {{ errorMessage }}
                        </div>
                    </div>
                    <div v-show="changePwdMenu" class="filler01"></div>
                    <div v-show="changePwdMenu" class="change-pwd-btn-section">
                        <button @click="changePwd" class="save-pwd-btn">Save</button>
                        <div class="filler05"></div>
                        <button @click="cancelChangePwd" class="cancel-pwd-btn">Cancel</button>
                    </div>
                </div>
                <div class="filler02"></div>
                <button @click="logoutUser" class="logout-btn">Logout</button>
            </div>
            <div v-else class="main-container-bottom">
                <div class="profile-info-container">
                    <img src="@/assets/info.png" class="info-img" alt="info">
                    <h2 class="profile-info-txt">This is not an Area account. You cannot modify your account information here. </h2>
                </div>
                <button @click="logoutUser" class="logout-btn">Logout</button>
            </div>
        </div>
</template>

<script>

export default {
    name: 'AccountSection',
    data() {
        return {
            mobile: false,
            changePwdMenu: false,
            isAreaAccount: false,
            isGoogleAccount: false,
            isDiscordAccount: false,
            userEmail: '',
            userName: '',
            userAvatar: '',
            oldPwd: '',
            newPwd: '',
            confirmNewPwd: '',
            errorMessage: ''
        }
    },
    methods: {
        activateChangePwdMenu() {
            this.changePwdMenu = true;
        },

        splitString(input) {
            return input.split(';');
        },

        async getUserInformation() {
            try {
            const response = await this.$axios.get(`/auth/userinformation`, {
                headers: {
                    'X-User-Token': localStorage.getItem("token"),
                },
            });
            if (response.status === 200) {
                const result = this.splitString(response.data);
                this.userEmail = result[0];
                this.userName = result[1];
                this.userAvatar = this.userName.charAt(0);
            }
            } catch (error) {
                console.error(error);
            }
        },

        logoutUser() {
            localStorage.clear();
            window.location.href = this.$router.resolve({ name: 'login' }).href;
        },

        cancelChangePwd() {
            this.changePwdMenu = false;
            this.errorMessage = '';
        },

        async changePwd() {
            if (this.newPwd !== this.confirmNewPwd) {
                this.errorMessage = 'Passwords do not match';
                return;
            }
            this.errorMessage = '';
            try {
                const response = await this.$axios.post('/auth/reset-password', {
                    Password: this.oldPwd,
                    NewPassword: this.newPwd
                });
                this.changePwdMenu = false;
                console.log(response);
            } catch (error) {
                this.errorMessage = 'An error occurred. Please try again later.';
            }
        }
    },
    mounted() {
        if (localStorage.getItem('AccountType') === 'Area') {
            this.isAreaAccount = true;
            this.getUserInformation();
        } else if (localStorage.getItem('AccountType') === 'Google') {
            this.isGoogleAccount = true;
        } else if (localStorage.getItem('AccountType') === 'Discord') {
            this.isDiscordAccount = true;
        }

    }
}
</script>

<style>

.account-section-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    height: auto;
    width: 100%;
    background-color: #efefef;
    z-index: 1;
}

.account-avatar-container {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 6rem;
    height: 6rem;
    border-radius: 50%;
    background-color: #9fb7b9;
    border: 1px solid #969696;
    overflow: hidden;
    cursor: pointer;
}

.avatar-img {
    height: 100%;
    width: 100%;
    object-fit: cover;
}

.avatar-img:hover {
    opacity: 0.7;
}

.input-style1 {
    width: 13rem;
    height: 2rem;
    background-color: transparent;
    border: none;
    border-color: #bcc1ba;
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    outline: none;
}

.input-style1:focus {
    border-bottom: 2px solid #bcc1ba;
}

.img-style1 {
    width: 1.5rem;
    height: 1.5rem;
    margin-right: 1rem;
}

.user-name-container {
    display: flex;
    align-items: center;
    justify-content: start;
    background-color: transparent;
    height: auto;
    width: 17rem;
}

.user-pwd-container {
    display: flex;
    align-items: center;
    justify-content: start;
    background-color: transparent;
    height: auto;
    width: 17rem;
}

.email-txt {
    font-size: 2rem;
    color: #313030;
    margin: 0;
    padding: 0;
    font-family: 'inter', sans-serif;
    font-weight: 400;
}

.filler01 {
    width: 90%;
    height: 2rem;
    background-color: transparent;
    border: transparent;
}

.filler02 {
    width: 90%;
    height: 1rem;
    background-color: transparent;
    border: transparent;
}

.filler03 {
    width: 90%;
    height: 0.5rem;
    background-color: transparent;
    border: transparent;
}

.change-pwd-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #969696;
    border-radius: 5px;
    border: none;
    width: 7rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
}

.change-pwd-btn:hover {
    background-color: #bcc1ba;
}

.change-pwd-btn:active {
    background-color: #969696;
}

.hr-style01 {
    width: 90%;
    background-color: #bcc1ba;
}

.user-avatar-ini {
    font-size: 3rem;
    color: #313030;
    margin: 0;
    padding: 0;
    font-family: 'inter', sans-serif;
    font-weight: 400;
    cursor: pointer;
}

.profile-title-txt-style {
    font-size: 1rem;
    color: #313030;
    margin: 0;
    padding: 0;
    font-family: 'inter', sans-serif;
    font-weight: 500;
}

.section-container {
    display: flex;
    flex-direction: column;
    background-color: transparent;
    width: auto;
    height: auto;
}

.input-pwd-style1 {
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

.filler04 {
    width: 2rem;
    min-width: 1.5rem;
    margin-right: 1rem;
    height: 100%;
    background-color: transparent;
    border: transparent;
}

.change-pwd-btn-section {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: transparent;
    width: 17rem;
    height: 2rem;
}

.save-pwd-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #28728B;
    border-radius: 5px;
    border: none;
    width: 6rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 0.5rem;
    margin-bottom: 5px;
}

.save-pwd-btn:hover {
    background-color: #3a9cb1;
}

.save-pwd-btn:active {
    background-color: #2e7f8f;
}

.cancel-pwd-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #969696;
    border-radius: 5px;
    border: none;
    width: 6rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 0.5rem;
    margin-bottom: 5px;
}

.cancel-pwd-btn:hover {
    background-color: #bcc1ba;
}

.cancel-pwd-btn:active {
    background-color: #969696;
}

.filler05 {
    width: 1rem;
    height: 100%;
    background-color: transparent;
    border: transparent;
}

.error-message {
    color: red;
    font-family: 'inter', sans-serif;
    font-size: 14px;
    margin-top: 0.5rem;
}

.error-message-section {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: transparent;
    width: 17rem;
    height: auto;
}

.main-container-top {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    width: 25rem;
    height: 7rem;
    background-color: #bcc1ba;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
    border-bottom-color: black;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    border: 1px solid transparent;
    border-bottom-color: #000000;
}

.main-container-bottom {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: auto;
    width: 25rem;
    background-color: #efefef;
    border-bottom-left-radius: 10px;
    border-bottom-right-radius: 10px;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    gap: 0.5rem;
}

.logo-account {
    width: 5rem;
    height: 5rem;
    margin-left: 1rem;
    margin-right: 1.5rem;
}


.account-title-txt {
    font-size: 3rem;
    color: #313030;
    margin: 0;
    padding: 0;
    margin-left: 1.5rem;
    margin-right: 1rem;
    font-family: 'inter', sans-serif;
    font-weight: 400;
}

.info-img {
    width: 3.5rem;
    height: 3.5rem;
}

.profile-info-container {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: row;
    width: 80%;
    height: 100%;
    margin-top: 2rem;
    margin-bottom: 2rem;
}

.profile-info-txt {
    font-family: 'inter', sans-serif;
    font-size: medium;
    color: #313030;
    margin: 0;
    padding: 0;
    margin-left: 1rem;
    margin-right: 1rem;
}

.logout-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #b60404;
    border-radius: 5px;
    border: none;
    width: 7rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-bottom: 2rem;
}

.logout-btn:hover {
    background-color: #d90f0f;
}

.logout-btn:active {
    background-color: #a30303;
}

</style>