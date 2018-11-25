export default interface User {
  id: string;
  name: string;
  surname: string;
  imageId: string | null;
  city: string;
  isFriend: boolean | null;
}
