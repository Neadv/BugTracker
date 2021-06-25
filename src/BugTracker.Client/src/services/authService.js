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
      error: error.response.data.error
    };
  })
}

export function loadUser(){
  const token = getToken();
  if (token){
    setAuthorizationHeader(token.accessToken);
    return decodeToken(token.accessToken);
  }
  return null;
}


