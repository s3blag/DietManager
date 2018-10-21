import MealIngredientIdWithQuantity from "../meal-ingredient/mealIngredientIdWithQuantity";

export default interface MealCreation {
  Name: string;
  Description: string;
  PhotoId: string;
  Calories: number;
  IngredientsIdsWithQuantity: MealIngredientIdWithQuantity[];
}
