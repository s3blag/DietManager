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
  faBook,
  faArrowAltCircleRight,
  faUtensils,
  faNewspaper,
  faUserFriends,
  faArrowRight,
  faArrowLeft,
  faTrashAlt,
  faEdit,
  faSquare,
  faPenSquare,
  faPencilAlt,
  faStar
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(faAppleAlt);
library.add(faUserCircle);
library.add(faSearch);
library.add(faPlusCircle);
library.add(faBook);
library.add(faArrowAltCircleRight);
library.add(faUtensils);
library.add(faNewspaper);
library.add(faUserFriends);
library.add(faArrowRight);
library.add(faArrowLeft);
library.add(faTrashAlt);
library.add(faPencilAlt);
library.add(faStar);
Vue.component("font-awesome-icon", FontAwesomeIcon);

import BootstrapVue from "bootstrap-vue";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
Vue.use(BootstrapVue);

// import VModal from "vue-js-modal";
// Vue.use(VModal);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
