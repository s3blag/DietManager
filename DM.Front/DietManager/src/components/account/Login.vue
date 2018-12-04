<template>
  <div id="user-login" class="content-background">
    <h3 class="main-color">Login</h3>
    <div id="login-container">
      <div class="login-element">
        <div class="label">Username</div>
        <input type="text" minlength="4" v-model="loginForm.username">
      </div>
      <div class="login-element">
        <div class="label">Password</div>
        <input type="password" minlength="4" v-model="loginForm.password">
      </div>
      <div
        id="error-message"
        :class="validationError.length === 0 ? 'hidden' : ''"
      >{{validationError}}</div>
      <button class="button" :class="!valid ? 'disabled' : ''" @click="submit">Log in</button>
    </div>
  </div>
</template>


<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Modal from "@/components/common/Modal.vue";
import UserLogin from "@/ViewModels/user/userLogin";
import AuthService from "@/services/authService";

Component.registerHooks(["beforeRouteEnter"]);

@Component({
  components: {
    modal: Modal
  }
})
export default class Login extends Vue {
  private loginForm: UserLogin = { username: "", password: "" };
  private validationError: string = "";

  get valid() {
    return (
      this.loginForm.username.length > 1 &&
      this.loginForm.password.length > 1 &&
      !/\s/.test(this.loginForm.username)
    );
  }

  submit() {
    if (this.valid) {
      AuthService.signIn(
        this.loginForm,
        () => this.$router.replace({ name: "Home" }),
        error => (this.validationError = error)
      );
    }
  }
}
</script>

<style lang="less" scoped>
.disabled {
  background-color: grey !important;
}
#user-login {
  margin: 100px auto;
  border-radius: 8px;
  padding-top: 10px;
  width: 40%;
  max-width: 500px;
  height: 400px;
}
.label {
  position: relative;
  text-align: left;
  left: 0px;
}
.login-element {
  margin: 0 auto;
  width: 250px;
}
h3 {
  margin-top: 35px;
}
input {
  margin: 0 auto;
  width: 250px;
  border-radius: 5px;
  border-color: rgb(204, 204, 204);
  border-style: solid;
}
#login-container {
  margin-top: 25px;
  > * {
    margin-bottom: 25px;
  }
}
.button {
  width: 75px;
  height: 40px;
}
#error-message {
  position: relative;
  top: -10px;
  color: red;
  margin-bottom: 5px;
}
.hidden {
  visibility: hidden;
}
</style>
