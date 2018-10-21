import MealCreation from "@/ViewModels/meal/mealCreation";
import Axios from "axios";
import MealLookup from "@/ViewModels/meal/mealLookup";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";

export default class MealApiCaller {
  static get(
    mealGuid: string,
    successHandler: (mealLookupVM: MealLookup) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.get<MealLookup>(`/api/meal/${mealGuid}`)
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static add(
    mealCreationVM: MealCreation,
    successHandler: (createdMealGuid: string) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.post("/api/meal/add", mealCreationVM)
      .then(response => {
        successHandler(response.data as string);
      })
      .catch(error => errorHandler(error));
  }

  static getMealIngredients(
    mealGuid: string,
    successHandler: (mealIngredientsVM: MealIngredient[]) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.get<MealIngredient[]>(`/api/meal/${mealGuid}/MealIngredients`)
      .then(response => {
        successHandler(response.data as MealIngredient[]);
      })
      .catch(error => errorHandler(error));
  }
}
