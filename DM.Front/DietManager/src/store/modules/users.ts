import axios from "axios";
import IUser from "@/ViewModels/user";

export default {
  state: {
    user: null
  },
  mutations: {
    updateCurrentUser(state: any, user: IUser): void {
      state.user = user;
    }
  },
  getters: {},
  actions: {
    signIn({ commit }: any): void {
      axios
        .post("/api/sign-in")
        .then(result => commit("updateCurrentUser", result.data))
        // eslint-disable-next-line
        .catch(error => console.error(error));
    }
  }
};
