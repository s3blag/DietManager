import User from "./user";

export default interface LoggedInUser extends User {
  tokenExpirationDate: Date | null;
}
