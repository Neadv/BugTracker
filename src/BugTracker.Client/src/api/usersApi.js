import { api } from "./api";

const fetchUserByName = (username) => (
  api.get('users/' + username)
);

const fetchUsers = (activated = true) => (
  api.get('users', {
    params: {
      Activated: activated
    }
  })
);

const deleteUser = (username)=> (
  api.delete('users/' + username)
);

const createUser = (username, email, password) => (
  api.post('users/', {
    username,
    password,
    email
  })
);

const updateUser = (username, email, isActivated) => (
  api.put('users/', {
    username,
    email,
    isActivated
  })
);

export const usersApi = {
  fetchUserByName,
  fetchUsers,
  deleteUser,
  createUser,
  updateUser
}