export default interface MealPreview {
  id: string;
  imageId: string | null;
  name: string;
  calories: number;
  creationDate: Date;
}
