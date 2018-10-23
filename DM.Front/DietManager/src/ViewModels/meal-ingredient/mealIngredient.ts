import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredient {
  id: string;
  imageId: string;
  name: string;
  calories: number;
  nutritions: Nutritions;
}
