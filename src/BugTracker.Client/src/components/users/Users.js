import { Switch, Route } from "react-router-dom";
import { UserDetail } from "./UserDetail";
import { UsersPage } from "./UsersPage";

export const Users = () => {
  return (
    <Switch>
      <Route exact path="/users/">
        <UsersPage />
      </Route>
      <Route exact path="/users/:username">
        <UserDetail/>
      </Route>
    </Switch>
  );
}