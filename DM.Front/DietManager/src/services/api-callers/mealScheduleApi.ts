import WeeklyMealSchedule from "@/ViewModels/meal-schedule/weeklyMealSchedule";
import MealScheduleEntryCreation from "@/ViewModels/meal-schedule/mealScheduleEntryCreation";
import MealScheduleEntryUpdate from "@/ViewModels/meal-schedule/mealScheduleEntryUpdate";
import { Actions } from "@/ViewModels/enums/actions";
import BaseApiCaller from "./baseApiCaller";

export default class MealScheduleApiCaller extends BaseApiCaller {
  static get(
    selectedWeekStartDateIsoString: string,
    successHandler: (mealIngredientVM: WeeklyMealSchedule) => void,
    errorHandler: (error: string) => void = this.defaultErrorHandler
  ) {
    super.Axios.get(`/api/meal-schedule/week/${selectedWeekStartDateIsoString}`)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data as WeeklyMealSchedule);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static add(
    mealScheduleEntryCreation: MealScheduleEntryCreation,
    successHandler: (createdMealScheduleEntryGuid: string) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    super.Axios.post("/api/meal-schedule/entry", mealScheduleEntryCreation)
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data as string);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static delete(
    mealScheduleEntryId: string,
    successHandler: (arg: Actions) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    super.Axios.delete(`/api/meal-schedule/entry/${mealScheduleEntryId}`)
      .then(response => {
        if (response.status === 200) {
          successHandler(Actions.Delete);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  static update(
    mealScheduleEntryUpdate: MealScheduleEntryUpdate,
    successHandler: (actionType: Actions) => void,
    errorHandler: (error: Error | string) => void = this.defaultErrorHandler
  ) {
    super.Axios.patch("/api/meal-schedule/entry/", {
      id: mealScheduleEntryUpdate.id,
      newDate: mealScheduleEntryUpdate.newDate.toISOString()
    })
      .then(response => {
        if (response.status === 200) {
          successHandler(Actions.Update);
        } else {
          errorHandler(response.statusText);
        }
      })
      .catch(error => errorHandler(error));
  }

  private static defaultErrorHandler(error: Error | string) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
