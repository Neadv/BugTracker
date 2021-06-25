const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
const tokenKey = 'BugTrackerAuth';

export function decodeToken(jwtToken) {
  const data = jwtToken.split('.')[1];
  const payload = JSON.parse(atob(data));
  if (payload) {
    return {
      username: payload.sub,
      email: payload.email,
      role: payload[roleClaim]
    };
  }
  return null;
}

export function saveToken(token) {
  const tokenJSON = JSON.stringify(token);
  localStorage.setItem(tokenKey, tokenJSON);
}

export function clearToken(){
  localStorage.clear();
}

export function getToken() {
  const token = localStorage.getItem(tokenKey);
  if (token) {
    return JSON.parse(token);
  }
  return null;
}