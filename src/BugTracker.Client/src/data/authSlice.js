import { createSlice } from "@reduxjs/toolkit";
import { login } from "../services/authService";

const authSlice = createSlice({
  name: 'auth',
  initialState: {
    error: null,
    loading: false,
    user: null
  },
  reducers: {
    startAuthorize(state, action) {
      if (!state.loading) {
        state.loading = true;
      }
    },
    authorized(state, action) {
      if (state.loading) {
        state.loading = false;
        state.user = action.payload;
        state.error = null;
      }
    },
    authorizeError(state, action) {
      if (state.loading) {
        state.loading = false;
        state.user = null;
        state.error = action.payload;
      }
    },
    clearUser(state, action) {
      state.user = null;
      state.error = null;
    }
  }
});

export const { startAuthorize, authorized, authorizeError, clearUser } = authSlice.actions;
export const authReducer = authSlice.reducer;

export const loginUser = (username, password) => (
  async dispatch => {
    dispatch(startAuthorize());
    const result = await login(username, password);
    if (result){
      dispatch(authorized(result));
    } else {
      dispatch(authorizeError('error'));
    }
  }
)