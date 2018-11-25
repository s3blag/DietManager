import Axios from "axios";

export default class ImageApiCaller {
  static add(
    imageString: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.post("/api/image/add", {
      image: imageString
    })
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }

  static get(
    imageGuid: string,
    successHandler: (imageString: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    Axios.get("/api/image/" + imageGuid).then(response =>
      successHandler(response.data)
    );
  }

  private static defaultErrorHandler(error: Error | null) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
