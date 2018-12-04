import AxiosRef, { AxiosRequestConfig, AxiosInstance } from "axios";
import AuthService from "../authService";

export default class BaseApiCaller {
  static Axios: AxiosInstance = AxiosRef.create({});

  static initConfig() {
    this.Axios.interceptors.request.use(function(config: AxiosRequestConfig) {
      config.headers.common = AuthService.authHeader;
      config.headers["Content-Type"] = "application/json";
      return config;
    });

    this.Axios.interceptors.response.use(
      function(response) {
        if (response.status === 401) {
          alert("un");
          AuthService.logout();
          location.reload(true);
        }
        return response;
      },
      error => {
        if (error.response && error.response.status === 401) {
          AuthService.logout();
        }
        return Promise.reject(error);
      }
    );
  }
}
