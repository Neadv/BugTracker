import { createSlice } from "@reduxjs/toolkit";
import { loadUser, login } from "../services/authService";

const user = loadUser();
const authSlice = createSlice({
  name: 'auth',
  initialState: {
    error: null,
    isAuthorized: user ? true : false,
    loading: false,
    user: user
  },
  reducers: {
    startAuthorize(state, action) {
      if (!state.loading) {
        state.loading = true;
      }
    },
    authorized(state, action) {
      state.loading = false;
      state.user = action.payload;
      state.error = null;
      state.isAuthorized = true;
    },
    authorizeError(state, action) {
      state.loading = false;
      state.user = null;
      state.error = action.payload;
      state.isAuthorized = false;
    },
    clearUser(state, action) {
      state.user = null;
      state.error = null;
      state.isAuthorized = false;
    }
  }
});

export const { startAuthorize, authorized, authorizeError, clearUser } = authSlice.actions;
export const authReducer = authSlice.reducer;

export const loginUser = (username, password) => (
  async dispatch => {
    dispatch(startAuthorize());
    const result = await login(username, password);
    if (result.succeeded) {
      dispatch(authorized(result.user));
    } else {
      dispatch(authorizeError(result.error));
    }
  }
)