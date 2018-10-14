import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredientCreation {
  PhotoId?: number;
  Name: string;
  Calories: number;
  Nutritions: Nutritions;
}
