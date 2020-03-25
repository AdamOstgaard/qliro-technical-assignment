import './App.css';
import { Login } from './Login';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect
} from "react-router-dom";
import { isAuthenticated } from './_services/api.service';
import React from 'react';
import { Register } from './Register';
import { AllBooks } from './scenes/AllBooks';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <Router>
        <Switch>
          <Route exact path="/">
            {isAuthenticated() ? <AllBooks/> : <Redirect to="/login"></Redirect>}
          </Route>
          <Route path="/login">
            <Login/>
            </Route>
            <Route path="/register">
              <Register/>
            </Route>
          </Switch>
          </Router>
      </header>
    </div>
  );
}

export default App;
