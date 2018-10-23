import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredientCreation {
  imageId: string;
  quantity: number;
  name: string;
  calories: number;
  nutrition: Nutritions;
}
