import Axios from "axios";
import WeeklyMealSchedule from "@/ViewModels/meal-schedule/weeklyMealSchedule";
import MealScheduleEntryCreatiion from "@/ViewModels/meal-schedule/mealScheduleEntryCreation";

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
    mealScheduleEntryCreation: MealScheduleEntryCreatiion,
    successHandler: (createdMealScheduleEntryGuid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/meal-schedule/entry", mealScheduleEntryCreation)
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
