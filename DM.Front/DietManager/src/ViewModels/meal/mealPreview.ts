import User from "../user/user";

export default interface MealPreview {
  id: string;
  imageId: string | null;
  imageData: string | null;
  name: string;
  calories: number;
  numberOfUses: number;
  numberOfFavouriteMarks: number;
  creationDate: string;
  isFavourite: boolean | null;
  creator: User;
}
