import UserLogin from "@/ViewModels/user/userLogin";
import Axios from "axios";
import User from "@/ViewModels/user/user";
import LoggedInUser from "@/ViewModels/user/loggedInUser";
import UserRegistrationForm from "@/ViewModels/user/userRegistrationForm";

export default class AuthService {
  private static get signedInUser() {
    return JSON.parse(localStorage.getItem(
      "user"
    ) as string) as LoggedInUser | null;
  }

  static get isLoggedIn() {
    return (
      this.signedInUser !== null &&
      this.signedInUser.token &&
      this.signedInUser.token.expiration > new Date()
    );
  }

  static get loggedInUser() {
    if (this.isLoggedIn) {
      return this.signedInUser;
    } else {
      return null;
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
  }

  private static setUser(user: LoggedInUser) {
    if (user.token) {
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
        if (response.status === 400) {
          errorHandler(response.statusText);
        } else {
          successHandler(response.data);
        }
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
        errorHandler(error);
      });
  }

  private static sendGetLoggedUserRequest(
    onSuccess: (user: LoggedInUser) => void,
    onError: (errors: any) => void
  ) {
    Axios.get<LoggedInUser>("/api/user", this.authHeader)
      .then(response => {
        onSuccess(response.data);
      })
      .catch(error => {
        onError(error);
      });
  }

  private static sendDeleteAccountRequest(
    onSuccess: () => void,
    onError: (errors: any) => void
  ) {
    Axios.delete("/api/user", this.authHeader)
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }
}
