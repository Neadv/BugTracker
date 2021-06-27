import { useFormik } from "formik"
import { useState } from "react";
import { Form, Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import * as Yup from 'yup';
import { accountRegister } from "../../api/accountApi";

export const Register = () => {
  const [errors, setErrors] = useState([]);
  const [successful, setSuccessful] = useState(false);

  const handleSubmit = (values) => {
    accountRegister(values.username, values.email, values.password)
      .then(() => setSuccessful(true))
      .catch(error => {
        const status = error.response?.data?.status;
        if (status === 409 || status === 400) {
          setErrors(error.response?.data?.errors);
        }
      });
  };

  const formik = useFormik({
    initialValues: {
      email: '',
      username: '',
      password: '',
      confirmPassword: ''
    },
    validationSchema: Yup.object({
      username: Yup.string()
        .max(20, 'Must be 20 characters or less')
        .min(6, "Must be 6 characters or longer")
        .required('This field is required'),
      password: Yup.string()
        .matches(
          /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$/,
          "Must Contain 6 Characters, One Uppercase, One Lowercase, One Number and one special case Character"
        )
        .required('This field is require'),
      confirmPassword: Yup.string()
        .required('This field is required')
        .oneOf([Yup.ref('password'), null], 'Passwords must match'),
      email: Yup.string()
        .required('This field is required')
        .email('Invalid email')
    }),
    onSubmit: handleSubmit
  });

  if (successful) {
    return (
      <>
        <h3 className="text-center">Your account has been successfully created</h3>
        <div className="my-2">After activating your account, you can use it. You will receive a letter to the specified email.</div>
        <Link to="/account/login" className="text-center d-block">You will be able to log in using this link.</Link>
      </>
    );
  }

  return (
    <>
      <h3 className="text-center">Register</h3>
      <Form onSubmit={formik.handleSubmit}>
        {errors && (
          <ul className="text-danger">
            {errors.map((e, index) => <li key={index}>{e}</li>)}
          </ul>
        )}
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
          <Form.Label htmlFor="email">Email:</Form.Label>
          <Form.Control
            id="email"
            type="email"
            value={formik.values.email}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            placeholder="Enter your email"
          />
          {formik.touched.email && formik.errors.email ? <Form.Text className="text-danger">{formik.errors.email}</Form.Text> : null}
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
        <Form.Group>
          <Form.Label htmlFor="confirmPassword">Confrim password:</Form.Label>
          <Form.Control
            id="confirmPassword"
            type="password"
            value={formik.values.confirmPassword}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            placeholder="Please confirm password"
          />
          {formik.touched.confirmPassword && formik.errors.confirmPassword ? <Form.Text className="text-danger">{formik.errors.confirmPassword}</Form.Text> : null}
        </Form.Group>
        <div className="mb-2" style={{ fontSize: '0.8em' }}>Already have an account? <Link to="/account/login">Sign In</Link></div>
        <Button className="btn-block" type="submit">Sign Up</Button>
      </Form>
    </>
  );
}