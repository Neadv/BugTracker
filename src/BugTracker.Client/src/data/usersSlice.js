import { createSlice } from "@reduxjs/toolkit";
import { fetchUserByName } from "../api/usersApi";

const usersSlice = createSlice({
  name: 'users',
  initialState: {
    loading: false,
    errors: null,
    selectedUser: null,
    activatedUsers: [],
    notActivatedUsers: []
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
    loadActivatedUsers(state, action) {
      state.activatedUsers = action.payload;
      state.loading = false;
    },
    loadNotActivatedUsers(state, action) {
      state.notActivatedUsers = action.payload;
      state.loading = false;
    },
    deleteUser(state, action) {
      const username = action.payload;
      let array;
      let existingUser = state.activatedUsers.find(u => u.username === username);
      if (existingUser) {
        array = state.activatedUsers;
      } else {
        existingUser = state.notActivatedUsers.find(u => u.username === username);
        if (existingUser) {
          array = state.notActivatedUsers;
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
export const { startLoading, loadSelectedUser, loadActivatedUsers, loadNotActivatedUsers, deleteUser, loadError } = usersSlice.actions;

export const fetchSelectedUser = (username) => (
  async dispatch => {
    dispatch(startLoading());
    fetchUserByName(username)
      .then(res => {
        dispatch(loadSelectedUser(res.data));
      })
      .catch(error => {
        dispatch(loadError(["Not found"]));
      });
  }
)