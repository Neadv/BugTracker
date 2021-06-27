import { Redirect, Route } from "react-router-dom";
import { useAuth } from "../account/authHook";

export const PrivateRoute = ({ children, ...rest }) => {
  const auth = useAuth();
  return (
    <Route {...rest}>
      {auth.isAuthorized ? children : <Redirect to="/account/login" />}
    </Route>
  );
}