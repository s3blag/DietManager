import Axios from "axios";
import WeeklyMealSchedule from "@/ViewModels/meal-schedule/weeklyMealSchedule";
import MealScheduleEntryCreation from "@/ViewModels/meal-schedule/mealScheduleEntryCreation";
import MealScheduleEntryUpdate from "@/ViewModels/meal-schedule/mealScheduleEntryUpdate";
import { Actions } from "@/ViewModels/enums/actions";

export default class MealScheduleApiCaller {
  static get(
    selectedWeekStartDateIsoString: string,
    successHandler: (mealIngredientVM: WeeklyMealSchedule) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.get(`/api/meal-schedule/week/${selectedWeekStartDateIsoString}`)
      .then(response => {
        successHandler(response.data as WeeklyMealSchedule);
      })
      .catch(error => errorHandler(error));
  }

  static add(
    mealScheduleEntryCreation: MealScheduleEntryCreation,
    successHandler: (createdMealScheduleEntryGuid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/meal-schedule/entry", mealScheduleEntryCreation)
      .then(response => {
        successHandler(response.data as string);
      })
      .catch(error => errorHandler(error));
  }

  static delete(
    mealScheduleEntryId: string,
    successHandler: (arg: Actions) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.delete(`/api/meal-schedule/entry/${mealScheduleEntryId}`)
      .then(() => {
        successHandler(Actions.Delete);
      })
      .catch(error => errorHandler(error));
  }

  static update(
    mealScheduleEntryUpdate: MealScheduleEntryUpdate,
    successHandler: (actionType: Actions) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.patch("/api/meal-schedule/entry/", mealScheduleEntryUpdate)
      .then(() => {
        successHandler(Actions.Update);
      })
      .catch(error => errorHandler(error));
  }

  private static defaultErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
