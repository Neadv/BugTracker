import axios from "axios";
import { accountLogin, accountLogout, accountRefresh } from "../api/accountApi";
import { api, setAuthorizationHeader } from "../api/api";
import { clearToken, decodeToken, getToken, saveToken } from "./tokenService";

export function login(username, password) {
  return accountLogin(username, password).then(res => {
    saveToken(res.data);
    setAuthorizationHeader(res.data.accessToken);
    const user = decodeToken(res.data.accessToken);
    return {
      user: user,
      succeeded: true
    };
  }).catch(error => {
    clearToken();
    return {
      user: null,
      succeeded: false,
      error: error.response?.data?.error
    };
  })
}

export function logout() {
  const token = getToken();
  clearToken();
  return accountLogout(token?.refreshToken);
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
    const token = getToken();
    if (error.response?.status !== 401 || !token) {
      return Promise.reject(error);
    }

    api.interceptors.response.eject(interceptor);

    return accountRefresh(token?.accessToken, token?.refreshToken).then(response => {
      saveToken(response.data);
      setAuthorizationHeader(response.data.accessToken);
      error.response.config.headers['Authorization'] = 'Bearer ' + response.data.accessToken;
      return axios(error.response.config);
    }).catch(error => {
      window.location.replace('/account/logout');
      clearToken();
    }).finally(setRefreshInterceptor);
  });

}

