import { BrowserRouter, Route, Switch } from "react-router-dom";
import { Account } from "./components/account/Account";

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/account" component={Account} />
        <Route>
          <h1 className="bg-warning text-center text-white m-2 p-2">Not Found</h1>
        </Route>
      </Switch>
    </BrowserRouter>
  );
}

export default App;
