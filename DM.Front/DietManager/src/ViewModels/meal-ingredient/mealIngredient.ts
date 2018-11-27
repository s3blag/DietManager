import Nutritions from "@/ViewModels/meal-ingredient/nutritions";

export default interface MealIngredient {
  id: string;
  imageId: string | null;
  name: string;
  calories: number;
  nutrition: Nutritions;
}
