import { Redirect, Route, Switch } from 'react-router-dom';
import { Login } from './Login';
import { Card } from 'react-bootstrap';
import './Account.scss';

export function Account() {
  return (
    <div className="account-wrapper">
      <Card className="shadow">
        <Card.Body>
          <Switch>
            <Route exact path="/account/login" component={Login} />
            <Route exact path="/account/register" />
            <Route>
              <Redirect to="/account/login" />
            </Route>
          </Switch>
        </Card.Body>
      </Card>
    </div>
  );
}