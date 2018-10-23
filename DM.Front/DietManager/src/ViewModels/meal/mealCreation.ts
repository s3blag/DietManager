import MealIngredientIdWithQuantity from "../meal-ingredient/mealIngredientIdWithQuantity";

export default interface MealCreation {
  name: string;
  description: string;
  imageId: string;
  calories: number;
  ingredientsIdsWithQuantity: MealIngredientIdWithQuantity[];
}
