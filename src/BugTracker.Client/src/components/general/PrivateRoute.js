import { Redirect, Route } from "react-router-dom";
import { useAuth } from "../account/authHook";

export const PrivateRoute = ({ component: Component, ...rest }) => {
  const auth = useAuth();
  return (
    <Route {...rest}
      render={props =>
        auth.isAuthorized ? (
          <Component {...props} />
        ) : (<Redirect to={{
          pathname: "/account/login",
          state: { from: props.location }
        }}
        />
        )
      }
    />);
}