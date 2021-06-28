import { api } from "./api";

export const fetchUserByName = (username) => (
  api.get('users/' + username)
);

export const fetchUsers = (activated = true) => (
  api.get('users', {
    params: {
      Activated: activated
    }
  })
);

export const deleteUserApi = (username)=> (
  api.delete('users/' + username)
);

export const createUserApi = (username, email, password) => (
  api.post('users/', {
    username,
    password,
    email
  })
);