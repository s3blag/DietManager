import Axios from "axios";
import GroupedAchievements from "@/ViewModels/achievement/groupedAchievements";
import UserAchievements from "@/ViewModels/achievement/userAchievements";

export default class FriendsApiCaller {
  static markAsRead(
    achievementIds: string[],
    successHandler: () => void,
    errorHandler: (errorCode: number | string) => void = this
      .defaultErrorHandler
  ) {
    Axios.post("/api/achievements/my-achievements/mark-as-read", achievementIds)
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
    Axios.get<UserAchievements>("/api/achievements/my-achievements")
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
