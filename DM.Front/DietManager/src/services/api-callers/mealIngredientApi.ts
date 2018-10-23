import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import Axios from "axios";
import MealIngredientCreation from "@/ViewModels/meal-ingredient/mealIngredientCreation";

export default class MealIngredientApiCaller {
  static get(
    mealIngredientGuid: string,
    successHandler: (mealIngredientVM: MealIngredient) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.get(`/api/mealIngredient/${mealIngredientGuid}`)
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
    Axios.post("/api/mealIngredient/add", mealIngredientCreationVM)
      .then(response => {
        successHandler(response.data as string);
      })
      .catch(error => errorHandler(error));
  }

  private static defaultErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
