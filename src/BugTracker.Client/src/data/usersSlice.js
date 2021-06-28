import { createSlice } from "@reduxjs/toolkit";
import { usersApi } from "../api/usersApi";

const usersSlice = createSlice({
  name: 'users',
  initialState: {
    loading: false,
    errors: null,
    message: null,
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
      if (action.payload.isActive){
        state.activeUsers = action.payload.users;
      } else{
        state.unactiveUsers = action.payload.users;
      }
      state.loading = false;
    },
    addUser(state, action) {
      state.users.push({ ...action.payload.user, isActivated: action.payload.isActivated })
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
export const { startLoading, loadSelectedUser, loadUsers, deleteUser, loadError } = usersSlice.actions;

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

export const deleteUserAction = (username) => (
  dispatch => {
    usersApi.deleteUser(username)
      .then(res => dispatch(deleteUser(username)))
      .catch(error => dispatch(loadError("Couldn't delete user")));
  }
)