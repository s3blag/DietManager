import Axios from "axios";
import User from "@/ViewModels/user/user";
import IndexedResult from "@/ViewModels/wrappers/indexedResult";
import FriendInvitation from "@/ViewModels/user/friendInvitation";
import UserActivity from "@/ViewModels/user/userActivity";

export default class FriendsApiCaller {
  static getFriends(
    lastReturnedFriend: IndexedResult<User> | null,
    successHandler: (indexedResult: IndexedResult<User[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<User[]>>(
      "/api/friends/my-friends",
      lastReturnedFriend
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static sendInvitation(
    invitedUserId: string,
    successHandler: () => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/friends/invite", { invitedUserId: invitedUserId })
      .then(() => {
        successHandler();
      })
      .catch(error => errorHandler(error));
  }

  static getInvitations(
    lastReturnedInvitation: IndexedResult<FriendInvitation> | null,
    successHandler: (indexedResult: IndexedResult<FriendInvitation[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<FriendInvitation[]>>(
      "/api/friends/invitations",
      lastReturnedInvitation
    )
      .then(response => {
        successHandler(response.data);
      })
      .catch(error => {
        errorHandler(error);
      });
  }

  static acceptInvitation(
    invitingUserId: string,
    successHandler: () => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/friends/invitations/accept", invitingUserId)
      .then(() => {
        successHandler();
      })
      .catch(error => errorHandler(error));
  }

  static ignoreInvitation(
    invitingUserId: string,
    successHandler: () => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/friends/invitations/ignore", invitingUserId)
      .then(() => {
        successHandler();
      })
      .catch(error => errorHandler(error));
  }

  static removeFromFriends(
    friendId: string,
    successHandler: () => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.delete(`/api/friends/remove/${friendId}`)
      .then(() => {
        successHandler();
      })
      .catch(error => errorHandler(error));
  }

  static getNewsFeed(
    lastReturnedActivity: IndexedResult<UserActivity> | null,
    successHandler: (indexedResult: IndexedResult<UserActivity[]>) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post<IndexedResult<UserActivity[]>>(
      "/api/friends/news-feed",
      lastReturnedActivity
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
