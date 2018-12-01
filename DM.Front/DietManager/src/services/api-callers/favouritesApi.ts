import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealPreview from "@/ViewModels/meal/mealPreview";
import BaseApiCaller from "./baseApiCaller";

export default class FavouritesApiCaller extends BaseApiCaller {
  static add(
    mealId: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post("/api/favourites", {
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
    super.Axios.delete(`/api/favourites/${mealId}`)
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }

  static get(
    lastReturnedMealPreview: IndexedResult<MealPreview> | null,
    successHandler: (indexedResult: IndexedResult<MealPreview[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post<IndexedResult<MealPreview[]>>(
      "/api/favourites/get",
      lastReturnedMealPreview,
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
