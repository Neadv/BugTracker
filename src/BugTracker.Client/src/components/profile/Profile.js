import { Row, Col } from "react-bootstrap";
import { useAuth } from "../account/authHook";
import { Form } from "react-bootstrap";
import { Wrapper } from "../general/Wrapper";
import { ChangePassword } from "./ChangePassword";

export const Profile = () => {
  const auth = useAuth();
  const user = auth.user;

  return (
    <Wrapper title="Profile" lg center>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Username:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={user.username} />
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Email:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={user.email} />
        </Col>
      </Form.Group>
      <Form.Group as={Row}>
        <Form.Label column md="2">
          Role:
        </Form.Label>
        <Col md="10">
          <Form.Control readOnly plaintext value={user.role} />
        </Col>
      </Form.Group>
      <Wrapper title="Change password" center style={{ maxWidth: "500px"}}>
        <ChangePassword />
      </Wrapper>
    </Wrapper>
  );
};