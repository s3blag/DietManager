export default interface GroupedAchievements {
  mealAchievement: { [key: string]: number[] };
  mealIngredientAchievement: { [key: string]: number[] };
  userAchievement: { [key: string]: number[] };
  mealScheduleAchievement: { [key: string]: number[] };
  friendAchievement: { [key: string]: number[] };
  any: boolean;
}
