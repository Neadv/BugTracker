import { createSlice } from "@reduxjs/toolkit";
import { usersApi } from "../api/usersApi";

const usersSlice = createSlice({
  name: 'users',
  initialState: {
    loading: false,
    errors: null,
    selectedUser: null,
    activeUsers: [],
    unactiveUsers: [],
  },
  reducers: {
    startLoading(state, action) {
      state.loading = true;
      state.errors = null;
    },
    loadSelectedUser(state, action) {
      state.selectedUser = action.payload;
      state.loading = false;
    },
    loadUsers(state, action) {
      if (action.payload.isActive) {
        state.activeUsers = action.payload.users;
      } else {
        state.unactiveUsers = action.payload.users;
      }
      state.loading = false;
    },
    addUser(state, action) {
      state.activeUsers.push(action.payload)
      state.loading = false;
    },
    deleteUser(state, action) {
      const username = action.payload;
      let array;
      let existingUser = state.activeUsers.find(u => u.username === username);
      if (existingUser) {
        array = state.activeUsers;
      } else {
        existingUser = state.unactiveUsers.find(u => u.username === username);
        if (existingUser) {
          array = state.unactiveUsers;
        }
      }
      if (array) {
        const index = array.indexOf(existingUser);
        array.splice(index, 1);
      }
    },
    loadError(state, action) {
      state.loading = false;
      state.errors = action.payload;
      state.activatedUsers = [];
      state.notActivatedUsers = [];
      state.selectedUser = null;
    }
  }
});

export const usersReducer = usersSlice.reducer;
export const { startLoading, loadSelectedUser, loadUsers, deleteUser, loadError, addUser } = usersSlice.actions;

export const fetchSelectedUser = (username) => (
  dispatch => {
    dispatch(startLoading());
    usersApi.fetchUserByName(username)
      .then(res => {
        dispatch(loadSelectedUser(res.data));
      })
      .catch(error => {
        dispatch(loadError(["Not found"]));
      });
  }
)

export const fetchUsersAction = (isActive = true) => (
  dispatch => {
    dispatch(startLoading());
    usersApi.fetchUsers(isActive)
      .then(res => dispatch(loadUsers({ isActive, users: res.data })))
      .catch(error => dispatch(loadError('Error')));
  }
)

export const deleteUserAction = (user) => (
  dispatch => {
    if (!user.roles.includes('admin')) {
      usersApi.deleteUser(user.username)
        .then(res => dispatch(deleteUser(user.username)))
        .catch(error => dispatch(loadError("Couldn't delete user")));
    }
  }
)

export const createUserAction = (user) => (
  dispatch => {
    dispatch(startLoading());
    usersApi.createUser(user.username, user.email, user.password)
      .then(res => dispatch(addUser({
        username: user.username,
        email: user.email,
        roles: []
      })))
      .catch(error => dispatch(loadError(error.response?.data?.errors?.join(', '))));
  }
)

export const updateUserStatus = (user, isActivated) => (
  dispatch => {
    usersApi.updateUser(user.username, user.email, isActivated)
      .then(res => dispatch(deleteUser(user.username)))
      .catch(e => dispatch(loadError(e.response?.data?.errors?.join(', '))));
  }
)