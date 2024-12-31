<template>
    <body>
        <div class="account-section-container">
            <div class="account-section">
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
                        <img src="@/assets/user.png" class="img-style1">
                        <input type="text" class="input-style1" placeholder="Username" v-model="userName">
                    </div>
                </div>
                <div class="filler03"></div>
                <div class="section-container">
                    <h2 class="profile-title-txt-style">Password</h2>
                    <div class="filler03"></div>
                    <div class="user-pwd-container">
                        <img src="@/assets/key.png" class="img-style1">
                        <button class="change-pwd-btn">Change</button>
                    </div>
                </div>
            </div>
        </div>
    </body>
</template>

<script>
export default {
    name: 'AccountSection',
    data() {
        return {
            mobile: false,
            userEmail: 'test@test.com',
            userName: 'Pablo',
            userAvatar: ''
        }
    },
    methods: {
        getAvatarLetter() {
            this.userAvatar = this.userName.charAt(0);
        },
    },
    mounted() {
        this.getAvatarLetter();
    }
}
</script>

<style>

.account-section-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: auto;
    width: 100%;
    background-color: #efefef;
    z-index: 1;
}

.account-section {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 30rem;
    width: 25rem;
    background-color: #efefef;
    border-radius: 10px;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    gap: 0.5rem;
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
    margin-left: 1rem;
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

</style>