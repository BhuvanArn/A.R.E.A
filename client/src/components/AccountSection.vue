<template>
    <body>
        <div class="account-section-container">
            <div class="account-section">
                <span class="account-avatar-container">
                    <img src="@/assets/user.png" class="avatar-img">
                </span>
                <h2 class="email-txt"> {{ userEmail }} </h2>
                <div class="filler01"></div>
                <div class="user-name-container">
                    <img src="@/assets/user.png" class="img-style1">
                    <input type="text" class="input-style1" placeholder="Username" v-model="userName">
                </div>
                <div class="filler02"></div>
                <div class="user-pwd-container">
                    <img src="@/assets/key.png" class="img-style1">
                    <button class="change-pwd-btn">Change Password</button>
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
        }
    },
    methods: {
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
    width: 40rem;
    background-color: #efefef;
    border-radius: 10px;
    box-shadow: 1px 1px 10px 0px rgba(0, 0, 0, 0.1);
    gap: 0.5rem;
    z-index: 1;
}

.account-avatar-container {
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 50%;
    height: 7rem;
    width: 7rem;
    border: 2px solid rgb(0, 0, 0);
    background-color: transparent;
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

.img-style1 {
    width: 1.5rem;
    height: 1.5rem;
    margin-left: 1rem;
    margin-right: 1rem;
}

.user-name-container {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: transparent;
    height: auto;
    width: 20rem;
}

.user-pwd-container {
    display: flex;
    align-items: center;
    justify-content: start;
    background-color: transparent;
    height: auto;
    width: 20rem;
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

.change-pwd-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #46b1c9;
    border-radius: 5px;
    border: none;
    width: 12rem;
    height: 2rem;
    color: #efefef;
    font-family: 'inter', sans-serif;
    font-size: 1.1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 0.5rem;
    margin-bottom: 1rem;
}

.change-pwd-btn:hover {
    background-color: #3a9cb1;
}

.change-pwd-btn:active {
    background-color: #2e7f8f;
}

</style>