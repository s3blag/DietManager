<template>
  <div class="header-container">
    <router-link class="header-element" to="/">
      <font-awesome-icon class="site-logo main-color" icon="apple-alt"/>
    </router-link>
    <router-link class="header-element hover" to="/friend-manager">Friends</router-link>
    <router-link class="header-element hover" to="/my-schedule">Schedule</router-link>
    <router-link class="header-element hover" to="/meal-manager">Meals</router-link>
    <div class="float-right" v-if="isUserLoggedIn">
      <div class="header-element">Hi, {{username}}!</div>
      <router-link class="header-element" to="/user-panel">
        <font-awesome-icon v-if="!avatarId" class="user-avatar main-color" icon="user-circle"/>
      </router-link>
    </div>
    <div id="login-and-registration" class="float-right">
      <router-link class="header-element hover" :to="{name: 'Login'}">Login</router-link>
      <router-link class="header-element hover" :to="{name: 'Register'}">Register</router-link>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

import User from "@/ViewModels/user/user";
import AuthService from "@/services/authService";

@Component({})
export default class Header extends Vue {
  @Prop()
  private user!: User;

  get isUserLoggedIn() {
    return AuthService.isLoggedIn;
  }

  get username() {
    //return user.username;
    return "Sebastian";
  }

  get avatarId() {
    if (!this.user || !this.user.imageId) {
      return null;
    } else {
      return this.user.imageId;
    }
  }
}
</script>

<style scoped>
.header-container {
  background-color: #e6e4e4;
  margin: 10px;
  padding: 5px;
  overflow: hidden;
  border-radius: 10px;
}
.header-element {
  position: relative;
  top: 2px;
  float: left;
  color: #46474bef;
  margin: 5px;
  text-decoration: none;
  font-size: 1.1em;
  line-height: 30px;
}
.float-right {
  float: right;
}
.site-logo {
  position: relative;
  top: -3px;
  margin-left: 5px;
  width: 30px;
  height: 30px;
}
.user-avatar {
  margin-left: -2px;
  margin-right: 5px;
  width: 30px;
  height: 30px;
}
.hover:hover {
  border-bottom-width: 1px;
  border-bottom-style: solid;
}
@media screen and (max-width: 500px) {
  .header-container.header-element {
    float: none;
    display: block;
    text-align: left;
  }
  .user-info {
    float: none;
  }
}
</style>
