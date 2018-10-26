import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import Axios from "axios";
import MealIngredientCreation from "@/ViewModels/meal-ingredient/mealIngredientCreation";
import MealIngredientSearch from "@/ViewModels/meal-ingredient/mealIngredientSearch";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";

export default class MealIngredientApiCaller {
  static get(
    mealIngredientGuid: string,
    successHandler: (mealIngredientVM: MealIngredient) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.get(`/api/meal-ingredient/${mealIngredientGuid}`)
      .then(response => {
        successHandler(response.data as MealIngredient);
      })
      .catch(error => errorHandler(error));
  }

  static add(
    mealIngredientCreationVM: MealIngredientCreation,
    successHandler: (createdMealIngredientGuid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/meal-ingredient/add", mealIngredientCreationVM)
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
    Axios.post<IndexedResult<MealIngredient[]>>(
      "/api/meal-ingredient/search",
      lastReturnedIndexedSearch,
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
