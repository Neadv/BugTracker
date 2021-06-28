import { Form, Col, Row } from "react-bootstrap";

export const UserInfo = ({ user }) => {
  const role = user?.role ?? user?.roles?.join(','); 
  return (
    <>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Username:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={user?.username ?? ''} />
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Email:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={user?.email ?? ''} />
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Role:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={role ?? ''} />
        </Col>
      </Form.Group>
    </>
  );
}