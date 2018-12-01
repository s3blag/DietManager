<template>
  <div id="user-registration" class="content-background">
    <h3 class="main-color">Registration</h3>
    <div id="registration-container">
      <div class="registration-element">
        <div class="label">Username</div>
        <input type="text" minlength="4" v-model="registrationForm.username">
      </div>
      <div class="registration-element">
        <div class="label">Password</div>
        <input type="password" minlength="4" v-model="registrationForm.password">
      </div>
      <div class="registration-element">
        <div class="label">Repeat password</div>
        <input type="password" minlength="4" v-model="registrationForm.passwordRepeated">
      </div>
      <div class="registration-element">
        <div class="label">Name</div>
        <input type="text" minlength="4" v-model="registrationForm.name">
      </div>
      <div class="registration-element">
        <div class="label">Surname</div>
        <input type="text" minlength="4" v-model="registrationForm.surname">
      </div>
      <div class="registration-element">
        <div class="label">City</div>
        <input type="text" minlength="4" v-model="registrationForm.city">
      </div>
      <div
        id="error-message"
        :class="validationError.length === 0 ? 'hidden' : ''"
      >{{validationError}}</div>
      <button class="button" :class="!isValid ? 'disabled' : ''" @click="submit">Register</button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import UserRegistrationForm from "@/ViewModels/user/userRegistrationForm";
import AuthService from "@/services/authService";
Component.registerHooks(["beforeRouteEnter"]);

@Component
export default class Register extends Vue {
  private registrationForm: UserRegistrationForm = {
    username: "",
    password: "",
    passwordRepeated: "",
    name: "",
    surname: "",
    city: ""
  };
  private validationError: string = "";

  get isValid() {
    const form = this.registrationForm;
    return (
      form.username.length > 3 &&
      form.password.length > 3 &&
      form.passwordRepeated == form.password &&
      form.name.length > 1 &&
      form.surname.length > 1 &&
      form.city.length > 1
    );
  }

  submit() {
    AuthService.register(
      this.registrationForm,
      () => this.$router.replace({ name: "Home" }),
      error => (this.validationError = error)
    );
  }
}
</script>

<style lang="less" scoped>
#user-registration {
  margin: 100px auto;
  border-radius: 8px;
  padding-top: 10px;
  width: 40%;
  max-width: 500px;
  height: fit-content;
}
.label {
  position: relative;
  text-align: left;
  left: 0px;
}
.registration-element {
  margin: 0 auto;
  width: 250px;
}
.disabled {
  background-color: grey;
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
#registration-container {
  margin-top: 25px;
  > * {
    margin-bottom: 25px;
  }
}
.button {
  width: 75px;
  height: 40px;
  margin-bottom: 60px !important;
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
