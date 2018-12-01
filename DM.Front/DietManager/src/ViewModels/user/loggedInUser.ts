import User from "./user";
import Token from "./token";

export default interface LoggedInUser extends User {
  token: Token;
}
