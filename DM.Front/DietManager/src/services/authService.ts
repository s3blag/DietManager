import UserLogin from "@/ViewModels/user/userLogin";
import Axios from "axios";
import User from "@/ViewModels/user/user";
import LoggedInUser from "@/ViewModels/user/loggedInUser";
import UserRegistrationForm from "@/ViewModels/user/userRegistrationForm";
import { EventBus } from "@/services/eventBus";

export default class AuthService {
  private static getSignedInUser() {
    return JSON.parse(localStorage.getItem(
      "user"
    ) as string) as LoggedInUser | null;
  }
  static isLoggedIn() {
    return (
      this.getSignedInUser() !== null &&
      this.getSignedInUser()!.token &&
      new Date(this.getSignedInUser()!.token!.expiration) > new Date()
    );
  }

  static inititalize() {
    EventBus.$on("reload-user-data", () =>
      this.sendGetLoggedUserRequest(
        user => {
          this.setUser(user);
          EventBus.$emit("user-data-changed");
        },
        () => null
      )
    );
  }

  static getloggedInUser() {
    if (this.isLoggedIn()) {
      return this.getSignedInUser();
    } else {
      return {
        token: null,
        name: "",
        city: "",
        id: "",
        imageId: "",
        isFriend: null,
        surname: ""
      } as LoggedInUser;
    }
  }

  static signIn(
    userLogin: UserLogin,
    onSuccess: () => void,
    onError: (errors: any) => void
  ) {
    this.sendSignInRequest(
      userLogin,
      userLogin => {
        this.setUser(userLogin);
        EventBus.$emit("user-state-changed");
        onSuccess();
      },
      onError
    );
  }

  static register(
    userRegistration: UserRegistrationForm,
    onSuccess: (user: User) => void,
    onError: (errors: any) => void
  ) {
    this.sendRegistrationRequest(
      userRegistration,
      user => {
        this.setUser(user);
        EventBus.$emit("user-state-changed");
        onSuccess(user);
      },
      onError
    );
  }

  static reloadLoggedInUser(
    onSuccess: (user: User) => void,
    onError: () => void
  ) {
    this.sendGetLoggedUserRequest(loggedInUser => {
      this.setUser(loggedInUser);
      EventBus.$emit("user-data-changed");
      onSuccess(loggedInUser);
    }, onError);
  }

  static deleteAccount(
    onSuccess: () => void,
    onError: (errors: string[]) => void
  ) {
    this.sendDeleteAccountRequest(() => {
      this.logout();
      onSuccess();
    }, onError);
  }

  static get authHeader() {
    let user = JSON.parse(localStorage.getItem(
      "user"
    ) as string) as LoggedInUser;

    if (user && user.token) {
      return { Authorization: "Bearer " + user.token.value };
    } else {
      return {} as any;
    }
  }

  public static logout() {
    localStorage.removeItem("user");
    EventBus.$emit("user-state-changed");
    location.reload(true);
  }

  private static setUser(user: LoggedInUser) {
    if (user) {
      localStorage.setItem("user", JSON.stringify(user));
    }
  }

  private static sendSignInRequest(
    userLogin: UserLogin,
    successHandler: (loggedInUser: LoggedInUser) => void,
    errorHandler: (error: Error | string) => void
  ) {
    Axios.post<LoggedInUser>("/api/auth/login", userLogin)
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        if (error.response.status === 400) errorHandler(error.response.data);
      });
  }

  private static sendRegistrationRequest(
    userRegistration: UserRegistrationForm,
    successHandler: (loggedInUser: LoggedInUser) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.post<LoggedInUser>("/api/auth/register", userRegistration)
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        if (error.response.status === 400) errorHandler(error.response.data);
      });
  }

  private static sendGetLoggedUserRequest(
    onSuccess: (user: LoggedInUser) => void,
    onError: (errors: any) => void
  ) {
    Axios.get<LoggedInUser>("/api/user", {
      headers: this.authHeader
    })
      .then(response => {
        onSuccess(response.data);
      })
      .catch(error => {
        onError(error);
      });
  }

  private static sendDeleteAccountRequest(
    onSuccess: () => void | null,
    onError: (errors: any) => void
  ) {
    Axios.delete("/api/user", { headers: this.authHeader })
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }
}
