<template>
  <div id="user-panel">
    <div id="header" class="soft-border bottom">
      <span>
        <font-awesome-icon class="user-avatar main-color" icon="user-circle"/>
        <router-link class="link" :to="{ name: 'AccountManager'}">Profile</router-link>
      </span>
      <span>
        <font-awesome-icon class="site-logo main-color" icon="trophy"/>
        <router-link class="link" :to="{ name: 'UserAchievements'}">Achievements</router-link>
      </span>
    </div>
    <div id="content">
      <router-view/>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({
  components: {}
})
export default class UserPanel extends Vue {}
</script>

<style lang="less" scoped>
#user-panel {
  width: 80%;
  margin: 25px auto 15px auto;
}

#header {
  text-align: left;
  > * {
    margin-right: 10px;
  }
}

.link:hover {
  color: initial;
}

span > * {
  margin-right: 2px;
}

#content {
  display: flex;
  padding: 10px;
}
</style>

