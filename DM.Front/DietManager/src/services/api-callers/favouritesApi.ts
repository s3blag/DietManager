import Axios from "axios";

export default class FriendsApiCaller {
  static add(
    mealId: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/favourites", {
      mealId: mealId
    })
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }

  static delete(
    mealId: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.delete(`/api/favourites/${mealId}`)
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }

  private static defaultErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
