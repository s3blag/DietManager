import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import Axios from "axios";

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
  faStar,
  faMinusCircle,
  faUserPlus,
  faTrophy,
  faLemon,
  faEnvelope
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
library.add(faAppleAlt);
library.add(faUserCircle);
library.add(faSearch);
library.add(faPlusCircle);
library.add(faMinusCircle);
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
library.add(faUserPlus);
library.add(faTrophy);
library.add(faLemon);
library.add(faEnvelope);
Vue.component("font-awesome-icon", FontAwesomeIcon);

import BootstrapVue from "bootstrap-vue";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
Vue.use(BootstrapVue);

import "@/style/global.less";
import BaseApiCaller from "./services/api-callers/baseApiCaller";

Vue.config.productionTip = false;

BaseApiCaller.initConfig();

new Vue({
  router,
  render: h => h(App)
}).$mount("#app");
