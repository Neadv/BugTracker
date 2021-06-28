import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom"
import { fetchSelectedUser } from "../../data/usersSlice";
import { usersSelectors } from "../../selectors/userSelectors";
import { UserInfo } from "../general/UserInfo";
import { Wrapper } from "../general/Wrapper";

export const UserDetail = () => {
  const { username } = useParams();
  const user = useSelector(usersSelectors.selectedUser);
  const errors = useSelector(usersSelectors.errors);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchSelectedUser(username));
  }, [username, dispatch])

  return (
    <Wrapper title={`Profile - ${username}`} lg center>
      {errors && (
        <ul className="bg-danger m-2 text-white">
          {errors.map((e, index) => <li key={index} className="p-1">{e}</li>)}
        </ul>
      )}
      {!errors && <UserInfo user={user} />}
    </Wrapper>
  )
}