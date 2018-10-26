import MealPreview from "./mealPreview";

export default interface MealPreviewWithImage {
  mealPreview: MealPreview;
  imageData: string | null;
}
