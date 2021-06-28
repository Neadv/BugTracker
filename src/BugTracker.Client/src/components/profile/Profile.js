import { useAuth } from "../account/authHook";
import { UserInfo } from "../general/UserInfo";
import { Wrapper } from "../general/Wrapper";
import { ChangePassword } from "./ChangePassword";

export const Profile = () => {
  const auth = useAuth();
  const user = auth.user;

  return (
    <Wrapper title="Profile" lg center>
      <UserInfo user={user} />
      <Wrapper title="Change password" center style={{ maxWidth: "500px"}}>
        <ChangePassword />
      </Wrapper>
    </Wrapper>
  );
};