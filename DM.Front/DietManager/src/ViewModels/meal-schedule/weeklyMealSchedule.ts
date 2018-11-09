import MealScheduleEntry from "./mealScheduleEntry";

export default interface WeeklyMealSchedule {
  monday: MealScheduleEntry[];
  tuesday: MealScheduleEntry[];
  wednesday: MealScheduleEntry[];
  thursday: MealScheduleEntry[];
  friday: MealScheduleEntry[];
  saturday: MealScheduleEntry[];
  sunday: MealScheduleEntry[];
}
