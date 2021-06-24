import { useFormik } from "formik";
import { Form, Button } from "react-bootstrap";
import * as Yup from 'yup';

export function Login() {
  const handleSubmit = values => {
    alert(JSON.stringify(values, null, 2));
  };
  
  const formik = useFormik({
    initialValues:{
      username: '',
      password: ''
    },
    validationSchema: Yup.object({
      username: Yup.string()
        .max(20, 'Must be 20 characters or less')
        .min(4, "Must be 4 characters or longer")
        .required('This field is required'),
      password: Yup.string()
        .max(20, 'Must be 20 characters or less')
        .min(4, "Must be 4 characters or longer")
        .required('This field is require'),
    }),
    onSubmit: handleSubmit,
  });
  
  return (
    <>
      <h3 className="text-center">Login</h3>
      <Form onSubmit={formik.handleSubmit}>
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
        <Button className="btn-block" type="submit">Sign In</Button>
      </Form>
    </>
  );
}