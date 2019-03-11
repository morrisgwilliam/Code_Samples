import React, { Component } from "react";
import "./App.css";
import * as userService from "../src/Services/userService";
import { Route, Switch, withRouter } from "react-router-dom";
import LogIn from "./Components/LogIn";
import Register from "./Components/Register";
import Navigation from "./Components/Navigation";
import mainList from "./componentList";

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthorized: false,
      currentUser: {}
    };
  }

  getComponents = (route, index) => (
    <Route
      key={index}
      exact
      path={route.path}
      render={props => (
        <route.component {...this.state.currentUser} {...props} />
      )}
    />
  );

  componentDidMount() {
    this.getCurrentUser();
  }

  getCurrentUser = () => {
    userService
      .getCurrent()
      .then(this.getCurrentOnSuccess)
      .catch(this.getCurrentOnError);
  };
  getCurrentOnSuccess = response => {
    this.setState({
      isAuthorized: true,
      currentUser: response.data
    });
  };
  getCurrentOnError = () => {
    this.props.history.push("/login");
    this.setState({
      isAuthorized: false,
      currentUser: {}
    });
  };
  setAuthorized = () => {
    this.setState({
      isAuthorized: true
    });
  };
  logOut = () => {
    userService
      .logOut()
      .then(this.getCurrentUser)
      .catch(this.getCurrentOnError);
  };
  logOutOnError = response => {
    console.log(response);
  };
  getRoutes = () => {
    if (this.state.isAuthorized) {
      return (
        <Navigation
          {...this.props}
          currentUser={this.state.currentUser}
          logOut={this.logOut}
        >
          {mainList.map(this.getComponents)}
        </Navigation>
      );
    } else {
      return (
        <React.Fragment>
          <Route
            exact
            path="/login"
            render={props => (
              <LogIn {...props} setAuthorized={this.getCurrentUser} />
            )}
          />
          <Route
            exact
            path="/register"
            render={props => <Register {...props} />}
          />
        </React.Fragment>
      );
    }
  };
  render() {
    return <Switch>{this.getRoutes()}</Switch>;
  }
}

export default withRouter(App);
