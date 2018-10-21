import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredientCreation {
  PhotoId: string;
  Quantity: number;
  Name: string;
  Calories: number;
  Nutrition: Nutritions;
}
