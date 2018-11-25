<template>
  <div id="app">
    <header-bar></header-bar>
    <router-view />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Header from "./components/shared/HeaderBar.vue";
import Component from "vue-class-component";

@Component({
  components: {
    "header-bar": Header
  }
})
export default class App extends Vue {}
</script>


<style lang="less">
#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>

