import { api } from "../api/api";

export async function login(username, password) {
  try {
    const result = await api.post('account/token', {
      username,
      password
    });
    return result.data;
  } catch {
    return null;
  }
}