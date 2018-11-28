import User from "./user";
import GroupedAchievements from "../achievement/groupedAchievements";

export default interface FriendWithAchievements {
  user: User;
  achievements: GroupedAchievements;
}
