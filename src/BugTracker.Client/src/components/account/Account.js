import { Redirect, Route, Switch } from 'react-router-dom';
import { Login } from './Login';
import { Card } from 'react-bootstrap';
import { useDispatch } from 'react-redux';
import './Account.scss';
import { loginUser } from '../../data/authSlice';

export function Account() {
  const dispatch = useDispatch();
  
  const login = (username, password) => {
    dispatch(loginUser(username, password));
  }

  return (
    <div className="account-wrapper">
      <Card className="shadow">
        <Card.Body>
          <Switch>
            <Route exact path="/account/login">
              <Login callback={login}/>
            </Route>
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