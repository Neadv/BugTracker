import { useFormik } from "formik";
import { Form, Button } from "react-bootstrap";
import { Redirect } from "react-router";
import { Link } from "react-router-dom";
import * as Yup from 'yup';

export function Login({ login, error, isAuthorized, loading }) {
  const handleSubmit = values => {
    login(values.username, values.password);
  };

  const formik = useFormik({
    initialValues: {
      username: '',
      password: ''
    },
    validationSchema: Yup.object({
      username: Yup.string()
        .required('This field is required'),
      password: Yup.string()
        .required('This field is require'),
    }),
    onSubmit: handleSubmit,
  });

  return isAuthorized ? <Redirect to="/" /> : (
    <>
      <h3 className="text-center">Login</h3>
      <Form onSubmit={formik.handleSubmit}>
        {error && <div className="mb-2 text-danger">{error}</div>}
        <Form.Group>
          <Form.Label htmlFor="username">Username:</Form.Label>
          <Form.Control
            id="username"
            type="text"
            value={formik.values.username}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            placeholder="Enter your username"
          />
          {formik.touched.username && formik.errors.username ? <Form.Text className="text-danger">{formik.errors.username}</Form.Text> : null}
        </Form.Group>
        <Form.Group>
          <Form.Label htmlFor="password">Password:</Form.Label>
          <Form.Control
            id="password"
            type="password"
            value={formik.values.password}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            placeholder="Enter your password"
          />
          {formik.touched.password && formik.errors.password ? <Form.Text className="text-danger">{formik.errors.password}</Form.Text> : null}
        </Form.Group>
        <div className="mb-2" style={{fontSize: '0.8em'}}>Don't have an account? <Link to="/account/register">Sign Up</Link></div>
        <Button className="btn-block" type="submit" disabled={loading}>Sign In</Button>
      </Form>
    </>
  )
}