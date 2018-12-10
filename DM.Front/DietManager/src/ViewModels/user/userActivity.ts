import User from "./user";
import MealPreview from "../meal/mealPreview";
import MealIngredient from "../meal-ingredient/mealIngredient";
import Achievement from "../achievement/achievement";

export default interface UserActivity {
  id: number;
  user: User;
  meal: MealPreview;
  mealIngredient: MealIngredient;
  favourite: MealPreview;
  friend: User;
  achievement: Achievement;
  activityDate: Date;
}
