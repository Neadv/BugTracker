import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux"
import { createUserAction, deleteUserAction, fetchUsersAction, updateUserStatus } from "../../data/usersSlice";
import { usersSelectors } from "../../selectors/userSelectors"
import { Wrapper } from "../general/Wrapper"
import { UserCreater } from "./UserCreator";
import { UsersList } from "./UsersList";

export const UsersPage = () => {
  const [isActive, setActive] = useState(true);
  const users = useSelector(usersSelectors.users(isActive));
  const loading = useSelector(usersSelectors.loading);
  const errors = useSelector(usersSelectors.errors)
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(fetchUsersAction(isActive));
  }, [dispatch, isActive]);

  useEffect(() => {
    if (errors){
      window.scrollTo(0, 0);
    }
  }, [errors])

  const addUser = (user) => {
    dispatch(createUserAction(user));
  }

  return (
    <Wrapper title="Users" center lg>
      {errors && <div className="m-2 p-2 bg-danger text-white rounded">{errors}</div>}
      <UserCreater loading={loading} errors={errors} addUser={addUser} />
      <select className="form-control my-3"
        value={isActive ? 'active' : 'unactive'}
        onChange={(e) => setActive(e.target.value === 'active')}>
        <option value="active">Active users</option>
        <option value="unactive">Unactive users</option>
      </select>
      <UsersList users={users} isActive={isActive} 
        onDelete={(user) => dispatch(deleteUserAction(user))} 
        onUpdate={(user, isActive) => dispatch(updateUserStatus(user, isActive))} />
    </Wrapper>
  )
}