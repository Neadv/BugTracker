import axios from "axios";

export const api = axios.create({
    baseURL: 'http://localhost:5000/api/'
});

export const setAuthorizationHeader = (token) => {
    api.defaults.headers = { Authorization: `Bearer ${token}` };
}

export const clearAuthorizationHeader = () => {
    api.defaults.headers = {};
}