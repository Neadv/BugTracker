import { api } from "./api";

export const accountLogin = (username, password) => (
    api.post('account/token', {
        username, password
    })
);

export const accountRegister = (username, email, password) => (
    api.post('account/register', { username, email, password })
);

export const accountRefresh = (accessToken, refreshToken) => (
    api.post('account/refresh', {
        expiredToken: accessToken,
        refreshToken: refreshToken
    })
);

export const accountLogout = (refreshToken) => (
    api.post('account/logout', {
        refreshToken: refreshToken
    })
);

export const accountChangePassword = (password, newPassword) => (
    api.post('account/changePassword', { 
        password, 
        newPassword
    })
)