import axios from "axios";
import { api, clearAuthorizationHeader, setAuthorizationHeader } from "../api/api";
import { clearToken, decodeToken, getToken, saveToken } from "./tokenService";

export function login(username, password) {
  return api.post('account/token', {
    username, password
  }).then(res => {
    saveToken(res.data);
    setAuthorizationHeader(res.data.accessToken);
    const user = decodeToken(res.data.accessToken);
    return {
      user: user,
      succeeded: true
    };
  }).catch(error => {
    clearToken();
    clearAuthorizationHeader();
    return {
      user: null,
      succeeded: false,
      error: error.response?.data?.error
    };
  })
}

export function loadUser() {
  setRefreshInterceptor();
  const token = getToken();
  if (token) {
    setAuthorizationHeader(token.accessToken);
    return decodeToken(token.accessToken);
  }
  return null;
}

export function setRefreshInterceptor() {
  const interceptor = api.interceptors.response.use(res => res, error => {
    if (error.response?.status !== 401) {
      return Promise.reject(error);
    }

    api.interceptors.response.eject(interceptor);

    const token = getToken();
    return api.post('account/refresh', {
      expiredToken: token.accessToken,
      refreshToken: token.refreshToken
    }).then(response => {
      saveToken(response.data);
      setAuthorizationHeader(response.data.accessToken);
      return axios(error.response.config);
    }).catch(error => {
      clearToken();
    }).finally(setRefreshInterceptor);
  });
}

