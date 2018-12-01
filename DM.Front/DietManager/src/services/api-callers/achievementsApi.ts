import UserAchievements from "@/ViewModels/achievement/userAchievements";
import BaseApiCaller from "./baseApiCaller";

export default class FriendsApiCaller extends BaseApiCaller {
  static markAsRead(
    achievementIds: string[],
    successHandler: () => void,
    errorHandler: (errorCode: number | string) => void = this
      .defaultErrorHandler
  ) {
    super.Axios.post(
      "/api/achievements/my-achievements/mark-as-read",
      achievementIds
    )
      .then(response => {
        if (response.status === 200) {
          successHandler();
        } else {
          errorHandler(response.status);
        }
      })
      .catch(error => {
        errorHandler(500);
      });
  }

  static get(
    successHandler: (achievements: UserAchievements) => void,
    errorHandler: (error: number) => void = this.defaultErrorHandler
  ) {
    super.Axios.get<UserAchievements>("/api/achievements/my-achievements")
      .then(response => {
        if (response.status === 200) {
          successHandler(response.data);
        } else {
          errorHandler(response.status);
        }
      })
      .catch(error => errorHandler(error));
  }
  private static defaultErrorHandler(error: any) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
