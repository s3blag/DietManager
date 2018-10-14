import MealIngredient from "@/ViewModels/meal-ingredient/mealIngredient";

export default interface MealCreation {
  Name: string;
  Description: string;
  ImageId?: number;
  Ingredients: string[];
}
