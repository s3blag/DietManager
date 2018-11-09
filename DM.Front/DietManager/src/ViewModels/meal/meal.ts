import MealIngredientWithQuantity from "../meal-ingredient/mealIngredientWithQuantity";

export default interface Meal {
  id: string;
  imageId: string | null;
  name: string;
  Calories: number;
  ingredients: MealIngredientWithQuantity[];
}
