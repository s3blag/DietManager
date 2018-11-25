import UserLogin from "@/ViewModels/user/userLogin";
import Axios from "axios";
import User from "@/ViewModels/user/user";
import UserRegistration from "@/ViewModels/user/passwordRegistration";
import LoggedInUser from "@/ViewModels/user/loggedInUser";

export default class AuthService {
  private static signedInUser: LoggedInUser | null = null;

  static get isLoggedIn() {
    return (
      this.signedInUser !== null &&
      this.signedInUser.tokenExpirationDate &&
      this.signedInUser.tokenExpirationDate > new Date()
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
        this.signedInUser = userLogin;
        onSuccess();
      },
      onError
    );
  }

  static register(
    userRegistration: UserRegistration,
    onSuccess: (user: User) => void,
    onError: (errors: any) => void
  ) {
    this.sendRegistrationRequest(
      userRegistration,
      user => {
        this.signedInUser = user;
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
      this.signedInUser = loggedInUser;
      onSuccess(loggedInUser);
    }, onError);
  }

  static deleteAccount(
    onSuccess: () => void,
    onError: (errors: string[]) => void
  ) {
    this.sendDeleteAccountRequest(() => {
      this.signedInUser = null;
      onSuccess();
    }, onError);
  }

  private static sendSignInRequest(
    userLogin: UserLogin,
    successHandler: (loggedInUser: LoggedInUser) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.post<LoggedInUser>("/api/user/sign-in", userLogin)
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  private static sendRegistrationRequest(
    userRegistration: UserRegistration,
    successHandler: (loggedInUser: LoggedInUser) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.post<LoggedInUser>("/api/user/register", userRegistration)
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
    Axios.get<LoggedInUser>("/api/user")
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
    Axios.delete("/api/user")
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }
}
