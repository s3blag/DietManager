import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import BaseApiCaller from "./baseApiCaller";
import UserActivity from "@/ViewModels/user/userActivity";

export default class AdminApiCaller extends BaseApiCaller {
  static markActivitiesAsSeen(
    activitiesIds: number[],
    successHandler: () => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post<string[]>(
      "/api/user/activities/mark-as-seen",
      activitiesIds
    )
      .then(() => {
        successHandler();
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static getUsersActivities(
    lastReturnedActivity: IndexedResult<UserActivity> | null,
    successHandler: (indexedResult: IndexedResult<UserActivity[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post<IndexedResult<UserActivity[]>>(
      "/api/user/activities",
      lastReturnedActivity
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static deleteUserAccount(
    userId: string,
    onSuccess: () => void,
    onError: (errors: any) => void = this.defaultErrorHandler
  ) {
    super.Axios.delete(`/api/user/${userId}`)
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }

  static deleteMeal(
    mealId: string,
    onSuccess: () => void,
    onError: (errors: any) => void = this.defaultErrorHandler
  ) {
    super.Axios.delete(`/api/meal/${mealId}`)
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }

  static deleteMealIngredient(
    mealIngredientId: string,
    onSuccess: () => void,
    onError: (errors: any) => void = this.defaultErrorHandler
  ) {
    super.Axios.delete(`/api/meal-ingredient/${mealIngredientId}`)
      .then(() => {
        onSuccess();
      })
      .catch(error => {
        onError(error);
      });
  }

  private static defaultErrorHandler(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
