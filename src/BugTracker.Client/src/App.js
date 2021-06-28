import { BrowserRouter, Route, Switch } from "react-router-dom";
import { Account } from "./components/account/Account";
import { PrivateComponent } from "./components/general/PrivateComponent";
import { PrivateRoute } from "./components/general/PrivateRoute";
import { Layout } from "./components/general/Layout";
import { Profile } from "./components/profile/Profile";
import { Users } from "./components/users/Users";

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/account">
          <Account />
        </Route>
        <PrivateRoute path="/">
          <Layout>
            <Switch>
              <Route exact path="/">
                <PrivateComponent />
              </Route>
              <Route path="/users">
                <Users />
              </Route>
              <Route exact path="/profile">
                <Profile />
              </Route>
              <Route>
                <h1 className="bg-warning text-center text-white m-2 p-2">Not Found</h1>
              </Route>
            </Switch>
          </Layout>
        </PrivateRoute>
      </Switch>
    </BrowserRouter>
  );
}

export default App;
