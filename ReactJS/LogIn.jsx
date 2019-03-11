import React from "react";
import PropTypes from "prop-types";
import Button from "@material-ui/core/Button";
import CssBaseline from "@material-ui/core/CssBaseline";
import FormControl from "@material-ui/core/FormControl";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import Input from "@material-ui/core/Input";
import InputLabel from "@material-ui/core/InputLabel";
import Paper from "@material-ui/core/Paper";
import Typography from "@material-ui/core/Typography";
import withStyles from "@material-ui/core/styles/withStyles";
import * as userService from "../Services/userService";
import { Link as RouterLink } from "react-router-dom";
import Link from "@material-ui/core/Link";
import styles from "../Styles/Materials/Form";
import RecipesSVG from "./RecipesSVG";
import { Formik, Form } from "formik";
import * as schemas from "../Schemas/accountSchemas";

const MyLink = props => <RouterLink {...props} to="/register" />;

class LogInForm extends React.Component {
  constructor(props) {
    super(props);
    this.validation = schemas.getLogInSchema;
    this.state = {};
    this.state.data = this.validation.initialValues;
  }
  logIn = values => {
    userService
      .logIn(values)
      .then(this.onLogInSuccess)
      .catch(this.onLogInError);
  };
  onLogInSuccess = () => {
    this.props.history.push("/");
    this.props.setAuthorized();
  };
  onLogInerror = response => {
    console.log(response);
  };
  render() {
    const { classes } = this.props;

    return (
      <main className={classes.main}>
        <CssBaseline />
        <Paper className={classes.paper}>
          <RecipesSVG class="st0 svgLogIn" viewBox="0 0 213 60" />
          <Typography component="h1" variant="h5">
            {this.props.location.state
              ? "Sign In With Your New Account"
              : "Sign In"}
          </Typography>
          <Formik
            initialValues={this.state.data}
            onSubmit={this.logIn}
            validationSchema={this.validation()}
            enableReinitialize={true}
          >
            {props => {
              const {
                values,
                handleChange,
                handleSubmit,
                touched,
                errors,
                isSubmitting,
                handleBlur
              } = props;
              return (
                <Form
                  onChange={handleChange}
                  onSubmit={handleSubmit}
                  className={classes.form}
                  action="#"
                  data-parsley-validate=""
                  noValidate=""
                >
                  <FormControl margin="normal" required fullWidth>
                    <InputLabel htmlFor="EmailAddress">
                      {errors.EmailAddress && touched.EmailAddress
                        ? errors.EmailAddress
                        : "Email Address"}
                    </InputLabel>
                    <Input
                      id="EmailAddress"
                      type="email"
                      name="EmailAddress"
                      placeholder="Email Address"
                      onChange={handleChange}
                      onBlur={handleBlur}
                      value={values.EmailAddress}
                      error={
                        errors.EmailAddress && touched.EmailAddress
                          ? true
                          : false
                      }
                    />
                  </FormControl>
                  <FormControl margin="normal" required fullWidth>
                    <InputLabel htmlFor="password">
                      {errors.Password && touched.Password
                        ? errors.Password
                        : "Password"}
                    </InputLabel>
                    <Input
                      name="Password"
                      type="Password"
                      id="Password"
                      placeholder="Password"
                      value={values.Password}
                      onChange={handleChange}
                      error={errors.Password && touched.Password ? true : false}
                    />
                  </FormControl>
                  <FormControlLabel
                    control={<Checkbox value="remember" color="primary" />}
                    label="Remember me"
                  />
                  <Link component={MyLink}>Dont have an account?</Link>
                  <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    color="primary"
                    className={classes.submit}
                    disabled={isSubmitting}
                  >
                    Sign in
                  </Button>
                </Form>
              );
            }}
          </Formik>
        </Paper>
      </main>
    );
  }
}

LogInForm.propTypes = {
  classes: PropTypes.object.isRequired
};
const LogIn = () => withStyles(styles)(LogInForm);
export default LogIn();
