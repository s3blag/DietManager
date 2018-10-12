<template>
  <div class="header-container">
    <router-link class="header-element" to="/">
      <font-awesome-icon class="site-logo" icon="apple-alt" />
    </router-link>
    <router-link class="header-element" to="/my-friends">
      Friends
    </router-link>
    <router-link class="header-element" to="/my-schedule">
      Schedule
    </router-link>
    <router-link class="header-element" to="/diet-book">
      Diet Book
    </router-link>
    <div class="user-info">
      <div class="header-element">
        Hi, {{username}}!
      </div>
      <router-link class="header-element" to="/user-panel">
        <font-awesome-icon class="user-avatar" icon="user-circle" />
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

import User from "../../ViewModels/user";

@Component({})
export default class Header extends Vue {
  @Prop()
  private user!: User;

  get username() {
    //return user.username;
    return "Sebastian";
  }
}
</script>

<style scoped>
.header-container {
  background-color: #e6e4e4;
  padding: 5px 5px;
  overflow: hidden;
  border-radius: 10px;
}
.header-element {
  float: left;
  color: #46474bef;
  padding: 5px;
  margin: 5px;
  text-decoration: none;
  font-size: 18px;
  line-height: 30px;
}
.user-info {
  float: right;
}
.site-logo {
  position: relative;
  margin-left: 5px;
  top: -1px;
  width: 30px;
  height: 30px;
}
.user-avatar {
  margin-left: -2px;
  margin-right: 5px;
  width: 30px;
  height: 30px;
}
@media screen and (max-width: 150px) {
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
