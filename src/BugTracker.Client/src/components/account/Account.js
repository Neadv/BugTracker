import { Redirect, Route, Switch } from 'react-router-dom';
import { Login } from './Login';
import { Card } from 'react-bootstrap';
import { useAuth } from './authHook';
import './Account.scss';

export function Account() {
  const auth = useAuth();

  return (
    <div className="account-wrapper">
      {auth.isAuthorized ? <Redirect to='/' /> : (
        <Card className="shadow">
          <Card.Body>
            <Switch>
              <Route exact path="/account/login">
                <Login login={auth.login} error={auth.error}/>
              </Route>
              <Route exact path="/account/register" />
              <Route>
                <Redirect to="/account/login" />
              </Route>
            </Switch>
          </Card.Body>
        </Card>)}
    </div>
  );
}