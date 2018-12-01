import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealIngredientCreation from "@/ViewModels/meal-ingredient/mealIngredientCreation";
import MealIngredientSearch from "@/ViewModels/meal-ingredient/mealIngredientSearch";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import BaseApiCaller from "./baseApiCaller";

export default class MealIngredientApiCaller extends BaseApiCaller {
  static get(
    mealIngredientGuid: string,
    successHandler: (mealIngredientVM: MealIngredient) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    super.Axios.get(`/api/meal-ingredient/${mealIngredientGuid}`)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data as MealIngredient);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static add(
    mealIngredientCreationVM: MealIngredientCreation,
    successHandler: (createdMealIngredientGuid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post("/api/meal-ingredient/add", mealIngredientCreationVM)
      .then(response => {
        successHandler(response.data as string);
      })
      .catch(error => errorHandler(error));
  }

  static search(
    lastReturnedIndexedSearch: IndexedResult<MealIngredientSearch> | null,
    successHandler: (indexedResult: IndexedResult<MealIngredient[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post<IndexedResult<MealIngredient[]>>(
      "/api/meal-ingredient/search",
      lastReturnedIndexedSearch
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  private static defaultErrorHandler(error: Error | string) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
