import Vue from "vue";
import Vuex from "vuex";

import robotsModule from "./modules/robots.js";
import usersModule from "./modules/users.js";

Vue.use(Vuex);

export default new Vuex.Store({
  
  modules: {
    robots: robotsModule,
    users: usersModule
  }
});