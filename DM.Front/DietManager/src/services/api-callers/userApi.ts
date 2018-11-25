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
      lastReturnedUserSearchResult
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static deleteUserAvatar(
    successHandler: () => void,
    errorHandler: (error: string[] | null) => void
  ) {
    Axios.delete("/api/user/avatar")
      .then(() => successHandler())
      .catch(errorHandler);
  }

  static upsertUserAvatar(
    newAvatarId: string,
    successHandler: (newAvatarId: string) => void,
    errorHandler: (error: string[] | null) => void
  ) {
    Axios.patch("/api/user/avatar", { imageId: newAvatarId })
      .then(response => successHandler(response.data))
      .catch(errorHandler);
  }

  private static defaultErrorHandler(error: Error | null) {
    // eslint-disable-next-line no-console
    console.error(error ? error : "null");
  }
}
