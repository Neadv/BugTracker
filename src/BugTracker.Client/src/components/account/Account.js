import { Redirect, Route, Switch } from 'react-router-dom';
import { Login } from './Login';
import { Card } from 'react-bootstrap';
import { useAuth } from './authHook';
import { PrivateRoute } from "../general/PrivateRoute";
import './Account.scss';
import { Logout } from './Logout';

export function Account() {
  const auth = useAuth();

  return (
    <div className="account-wrapper">
        <Card className="shadow">
          <Card.Body>
            <Switch>
              <Route exact path="/account/login">
                <Login {...auth}/>
              </Route>
              <Route exact path="/account/register" />
              <PrivateRoute exact path="/account/logout" component={Logout} />
              <Route>
                <Redirect to="/account/login" />
              </Route>
            </Switch>
          </Card.Body>
        </Card>
    </div>
  );
}