import { useDispatch, useSelector } from "react-redux"
import { clearUser, loginUser } from "../../data/authSlice";
import { clearToken } from "../../services/tokenService";
import { logout as authLogout } from "../../services/authService";

export const useAuth = () => {
  const auth = useSelector(state => state.auth);
  const dispatch = useDispatch();

  const login = (username, password) => {
    dispatch(loginUser(username, password));
  };

  const logout = () => {
    authLogout();
    clearToken();
    dispatch(clearUser());
  }

  return {
    ...auth,
    login,
    logout
  }
}