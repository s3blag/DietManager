import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faAppleAlt,
  faUserCircle,
  faSearch,
  faPlusCircle,
  faBook
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(faAppleAlt);
library.add(faUserCircle);
library.add(faSearch);
library.add(faPlusCircle);
library.add(faBook);
Vue.component("font-awesome-icon", FontAwesomeIcon);

import BootstrapVue from "bootstrap-vue";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
Vue.use(BootstrapVue);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
