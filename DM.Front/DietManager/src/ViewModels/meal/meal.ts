import MealIngredientWithQuantity from "../meal-ingredient/mealIngredientWithQuantity";
import User from "../user/user";

export default interface Meal {
  id: string;
  imageId: string | null;
  name: string;
  calories: number;
  description: string;
  creator: User;
  numberOfFavouriteMarks: number;
  numberOfUses: number;
  isFavourite: boolean | null;
  ingredients: MealIngredientWithQuantity[];
}
