import { useFormik } from "formik";
import { useState } from "react";
import { Form, Row, Col, Button } from "react-bootstrap";
import * as Yup from "yup";
import { accountChangePassword } from "../../api/accountApi";

export const ChangePassword = () => {
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState([]);
  const [success, setSuccess] = useState(null);

  const handleSubmit = (values) => {
    setLoading(true);
    accountChangePassword(values.password, values.newPassword)
      .then(() => {
        setLoading(false);
        setErrors([]);
        setSuccess("Password has been successfully changed");
      }).catch(error => {
        setLoading(false);
        setSuccess(null);
        setErrors(error.response?.data?.errors ?? []);
      });
  }

  const formik = useFormik({
    initialValues: {
      password: '',
      newPassword: '',
      confirmPassword: ''
    },
    validationSchema: Yup.object({
      password: Yup.string()
        .required('This field is require'),
      newPassword: Yup.string()
        .matches(
          /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$/,
          "Must Contain 6 Characters, One Uppercase, One Lowercase, One Number and one special case Character"
        )
        .required('This field is require'),
      confirmPassword: Yup.string()
        .required('This field is required')
        .oneOf([Yup.ref('newPassword'), null], 'Passwords must match'),
    }),
    onSubmit: handleSubmit
  })

  return (
    <Form style={{ fontSize: "16px" }} onSubmit={formik.handleSubmit}>
      <ul className="bg-danger text-white rounded mb-2">
        {errors.map((e, index) => <li key={index} className="p-1">{e}</li>)}
      </ul>
      {success && <div className="bg-success text-white rounded p-2 mb-2">{success}</div>}
      <Form.Group as={Row}>
        <Form.Label column md="4">Password:</Form.Label>
        <Col md="8">
          <Form.Control
            type="password"
            name="password"
            onBlur={formik.handleBlur}
            onChange={formik.handleChange}
            value={formik.values.password}
            placeholder="Enter your password"
          />
          {formik.touched.password && formik.errors.password ? <Form.Text className="text-danger">{formik.errors.password}</Form.Text> : null}
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="4">New Password:</Form.Label>
        <Col md="8">
          <Form.Control
            type="password"
            name="newPassword"
            onBlur={formik.handleBlur}
            onChange={formik.handleChange}
            value={formik.values.newPassword}
            placeholder="Enter new password"
          />
          {formik.touched.newPassword && formik.errors.newPassword ? <Form.Text className="text-danger">{formik.errors.newPassword}</Form.Text> : null}
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="4">Confirm Password:</Form.Label>
        <Col md="8">
          <Form.Control
            type="password"
            name="confirmPassword"
            onBlur={formik.handleBlur}
            onChange={formik.handleChange}
            value={formik.values.confirmPassword}
            placeholder="Please confirm new password"
          />
          {formik.touched.confirmPassword && formik.errors.confirmPassword ? <Form.Text className="text-danger">{formik.errors.confirmPassword}</Form.Text> : null}
        </Col>
      </Form.Group>
      <Button variant="primary" type="submit" disabled={loading}>Change password</Button>
    </Form>
  )
}