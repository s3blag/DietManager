import Axios from "axios";

export default class ImageApiCaller {
  static add(
    imageString: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void
  ) {
    Axios.post("/api/image/add", { Image: imageString })
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }
}
