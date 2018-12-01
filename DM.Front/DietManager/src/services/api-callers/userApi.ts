import Search from "@/ViewModels/meal/mealSearch";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import User from "@/ViewModels/user/user";
import BaseApiCaller from "./baseApiCaller";

export default class UserApiCaller extends BaseApiCaller {
  static search(
    lastReturnedUserSearchResult: IndexedResult<Search> | null,
    successHandler: (indexedResult: IndexedResult<User[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post<IndexedResult<User[]>>(
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
    super.Axios.delete("/api/user/avatar")
      .then(() => successHandler())
      .catch(errorHandler);
  }

  static upsertUserAvatar(
    newAvatarId: string,
    successHandler: (newAvatarId: string) => void,
    errorHandler: (error: string[] | null) => void
  ) {
    super.Axios.patch("/api/user/avatar", { imageId: newAvatarId })
      .then(response => successHandler(response.data))
      .catch(errorHandler);
  }

  private static defaultErrorHandler(error: Error | null) {
    // eslint-disable-next-line no-console
    console.error(error ? error : "null");
  }
}
