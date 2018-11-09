import Meal from "../meal/meal";

export default interface MealScheduleEntry {
  id: string;
  meal: Meal;
  date: Date;
}
