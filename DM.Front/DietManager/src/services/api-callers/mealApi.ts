import MealCreation from "@/ViewModels/meal/mealCreation";
import Axios from "axios";
import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";
import MealPreview from "@/ViewModels/meal/mealPreview";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import MealSearch from "@/ViewModels/meal/mealSearch";
import Meal from "@/ViewModels/meal/meal";

export default class MealApiCaller {
  static get(
    mealGuid: string,
    successHandler: (mealVM: Meal) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    Axios.get<Meal>(`/api/meal/${mealGuid}`)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static add(
    mealCreationVM: MealCreation,
    successHandler: (createdMealGuid: string) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/meal/add", mealCreationVM)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data as string);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static getMealIngredients(
    mealGuid: string,
    successHandler: (mealIngredientsVM: MealIngredient[]) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    Axios.get<MealIngredient[]>(`/api/meal/${mealGuid}/meal-ingredients`)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data as MealIngredient[]);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static getMealPreviews(
    lastReturnedMealPreview: IndexedResult<MealPreview> | null,
    successHandler: (indexedResult: IndexedResult<MealPreview[]>) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<MealPreview[]>>(
      "/api/meal/meal-previews",
      lastReturnedMealPreview
    )
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static search(
    lastReturnedIndexedSearch: IndexedResult<MealSearch>,
    successHandler: (indexedResult: IndexedResult<MealPreview[]>) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<MealPreview[]>>(
      "/api/meal/search",
      lastReturnedIndexedSearch
    )
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data);
        } else {
          errorHandler(response.statusText);
        }
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
