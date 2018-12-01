import BaseApiCaller from "./baseApiCaller";

export default class ImageApiCaller extends BaseApiCaller {
  static add(
    imageString: string,
    successHandler: (guid: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.post("/api/image/add", { image: imageString })
      .then(response => successHandler(response.data))
      .catch(error => errorHandler(error));
  }

  static get(
    imageGuid: string,
    successHandler: (imageString: string) => void,
    errorHandler: (error: Error) => void = this.defaultErrorHandler
  ) {
    super.Axios.get("/api/image/" + imageGuid).then(response =>
      successHandler(response.data)
    );
  }

  private static defaultErrorHandler(error: Error | null) {
    // eslint-disable-next-line no-console
    console.error(error);
  }
}
