<template>
  <div class="header-container">
    <router-link class="header-element" to="/">
      <font-awesome-icon class="site-logo main-color" icon="apple-alt"/>
    </router-link>
    <router-link class="header-element hover" to="/friend-manager">Friends</router-link>
    <router-link class="header-element hover" to="/my-schedule">Schedule</router-link>
    <router-link class="header-element hover" to="/meal-manager">Meals</router-link>
    <router-link
      v-if="user && user.isAdmin === true"
      class="header-element hover"
      to="/admin-panel"
    >Admin panel</router-link>
    <div
      id="user-elements"
      class="float-right"
      v-if="user && user.name"
      @click="userLogoClicked = !userLogoClicked"
    >
      <div class="header-element non-selectable">Hi, {{user.name}}!</div>
      <!-- <router-link class="header-element" to="/user-panel"> -->
      <div class="header-element">
        <image-wrapper :imageId="user.imageId">
          <template slot="placeholder">
            <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
          </template>
        </image-wrapper>
      </div>

      <ul id="account-options" class="content-background" v-show="userLogoClicked">
        <router-link tag="li" class="header-element" to="/user-panel">Account</router-link>
        <li class="header-element" @click="signOut">Sign Out</li>
      </ul>

      <!-- </router-link> -->
    </div>
    <div v-else id="login-and-registration" class="float-right">
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
import { EventBus } from "@/services/eventBus";
import LoggedInUser from "@/ViewModels/user/loggedInUser";
import ImageWrapper from "@/components/image/ImageWrapper.vue";
@Component({
  components: {
    "image-wrapper": ImageWrapper
  }
})
export default class Header extends Vue {
  private user: LoggedInUser | null = null;
  private userLogoClicked = false;
  mounted() {
    this.loadUser();
    EventBus.$on("user-state-changed", () => {
      this.loadUser();
      this.$router.replace({ name: "Home" });
    });
    EventBus.$on("user-data-changed", () => {
      this.loadUser();
    });
  }

  loadUser() {
    const user = AuthService.getloggedInUser();
    this.user = Object.assign({}, this.user, user);
  }

  get avatarId() {
    if (!this.user || !this.user.imageId) {
      return null;
    } else {
      return this.user.imageId;
    }
  }

  signOut() {
    AuthService.logout();
  }
}
</script>

<style lang="less" scoped>
#user-elements {
  position: relative;
}
#account-options {
  border-radius: 6px;
  border-style: solid;
  border-color: rgb(202, 202, 202);
  position: absolute;
  top: 50px;
  left: 0px;
  z-index: 10000;
  width: 100%;
  list-style-type: none;
  height: fit-content;
  margin: 0 auto;
  padding: 0px;
  > * {
    width: 100%;
    margin: 0px 0px 10px 0px;
  }
}
.header-element {
  cursor: pointer;
}
.non-selectable {
  -webkit-touch-callout: none;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.header-container {
  background-color: #e6e4e4;
  margin: 10px;
  padding: 5px;
  height: 65px;
  border-radius: 10px;
}
.header-element {
  position: relative;
  top: 5px;
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
.image-wrapper {
  position: relative;
  top: -5px;
  width: 45px;
  height: 45px;
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
