import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux"
import { deleteUserAction, fetchUsersAction } from "../../data/usersSlice";
import { usersSelectors } from "../../selectors/userSelectors"
import { Wrapper } from "../general/Wrapper"
import { UsersList } from "./UsersList";

export const UsersPage = () => {
  const [isActive, setActive] = useState(true);
  const users = useSelector(usersSelectors.users(isActive));
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchUsersAction(isActive));
  }, [dispatch, isActive]);

  return (
    <Wrapper title="Users" center lg>
      <select className="form-control my-3" 
        value={isActive ? 'active' : 'unactive'} 
        onChange={(e) => setActive(e.target.value === 'active')}>
        <option value="active">Active users</option>
        <option value="unactive">Unactive users</option>
      </select>
      <UsersList users={users} isActive={isActive} onDelete={(user) => dispatch(deleteUserAction(user.username))}/>
    </Wrapper>
  )
}