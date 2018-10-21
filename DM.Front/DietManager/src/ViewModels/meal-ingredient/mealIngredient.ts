import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredient {
  Id: string;
  PhotoId: string;
  Name: string;
  Calories: number;
  Nutritions: Nutritions;
}
