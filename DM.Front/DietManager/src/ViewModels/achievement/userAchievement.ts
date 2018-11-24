import Achievement from "./achievement";

export default interface UserAchievement {
  id: string;
  achievement: Achievement;
  seen: boolean;
}
