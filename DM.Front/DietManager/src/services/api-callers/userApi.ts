import Search from "@/ViewModels/meal/mealSearch";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import User from "@/ViewModels/user/user";
import Axios from "axios";

export default class UserApiCaller {
  static search(
    lastReturnedUserSearchResult: IndexedResult<Search> | null,
    successHandler: (indexedResult: IndexedResult<User[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<User[]>>(
      "/api/user/search",
      lastReturnedUserSearchResult,
      {
        headers: {
          "Content-Type": "application/json"
        }
      }
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  private static defaultErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
