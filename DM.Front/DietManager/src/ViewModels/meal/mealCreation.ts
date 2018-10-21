import MealIngredientIdWithQuantity from "../meal-ingredient/mealIngredientIdWithQuantity";

export default interface MealCreation {
  Name: string;
  Description: string;
  ImageId: string;
  Calories: number;
  IngredientsIdsWithQuantity: MealIngredientIdWithQuantity[];
}
