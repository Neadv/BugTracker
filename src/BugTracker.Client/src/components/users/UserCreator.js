import { useFormik } from "formik"
import { Form, Row, Col, Button } from "react-bootstrap";
import * as Yup from 'yup';
import { Wrapper } from "../general/Wrapper";

export const UserCreater = ({ addUser, loading }) => {
  const handleSubmit = (values, {resetForm}) => {
    addUser({
      username: values.username,
      email: values.email,
      password: values.password
    });
    resetForm({});
  }

  const formik = useFormik({
    initialValues: {
      username: '',
      email: '',
      password: '',
      confirmPassword: ''
    },
    validationSchema: Yup.object({
      username: Yup.string()
        .required('This field is required'),
      password: Yup.string()
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

  return (
    <Wrapper center title="Create New User">
      <Form onSubmit={formik.handleSubmit}>
        <Form.Group as={Row}>
          <Form.Label column md="2">Username:</Form.Label>
          <Col md="10">
            <Form.Control
              name="username"
              value={formik.values.username}
              onBlur={formik.handleBlur}
              onChange={formik.handleChange}
              placeholder="Enter username"
            />
            {formik.touched.username && formik.errors.username ? <Form.Text className="text-danger">{formik.errors.username}</Form.Text> : null}
          </Col>
        </Form.Group>
        <Form.Group as={Row}>
          <Form.Label column md="2">Email:</Form.Label>
          <Col md="10">
            <Form.Control
              name="email"
              type="email"
              value={formik.values.email}
              onBlur={formik.handleBlur}
              onChange={formik.handleChange}
              placeholder="Enter email"
            />
            {formik.touched.email && formik.errors.email ? <Form.Text className="text-danger">{formik.errors.email}</Form.Text> : null}
          </Col>
        </Form.Group>
        <Form.Group as={Row}>
          <Form.Label column md="2">Password:</Form.Label>
          <Col md="10">
            <Form.Control
              name="password"
              type="password"
              value={formik.values.password}
              onBlur={formik.handleBlur}
              onChange={formik.handleChange}
              placeholder="Enter password"
            />
            {formik.touched.password && formik.errors.password ? <Form.Text className="text-danger">{formik.errors.password}</Form.Text> : null}
          </Col>
        </Form.Group>
        <Form.Group as={Row}>
          <Form.Label column md="2">Confirm:</Form.Label>
          <Col md="10">
            <Form.Control
              name="confirmPassword"
              type="password"
              value={formik.values.confirmPassword}
              onBlur={formik.handleBlur}
              onChange={formik.handleChange}
              placeholder="Confirm password"
            />
            {formik.touched.confirmPassword && formik.errors.confirmPassword ? <Form.Text className="text-danger">{formik.errors.confirmPassword}</Form.Text> : null}
          </Col>
        </Form.Group>
        <Button variant="primary" type="submit" className="btn-block" disabled={loading}>Create</Button>
      </Form>
    </Wrapper>
  )
}