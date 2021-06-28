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

export const usersApi = {
  fetchUserByName,
  fetchUsers,
  deleteUser,
  createUser
}