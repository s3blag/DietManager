import MealCreation from "@/ViewModels/meal/mealCreation";
import Axios from "axios";
import MealLookup from "@/ViewModels/meal/mealLookup";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealPreview from "@/ViewModels/meal/mealPreview";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";

export default class MealApiCaller {
  static get(
    mealGuid: string,
    successHandler: (mealLookupVM: MealLookup) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
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
    errorHandler: (error: Error) => void = this.defaultErrorHandler
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
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.get<MealIngredient[]>(`/api/meal/${mealGuid}/meal-ingredients`)
      .then(response => {
        successHandler(response.data as MealIngredient[]);
      })
      .catch(error => errorHandler(error));
  }

  static getMealPreviews(
    lastReturnedMealPreview: MealPreview | null,
    successHandler: (indexedResult: IndexedResult<MealPreview>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<MealPreview>>(
      `/api/meal/meal-previews`,
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
