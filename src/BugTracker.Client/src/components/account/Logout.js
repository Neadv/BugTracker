import { useAuth } from "./authHook"
import { Redirect } from "react-router";

export const Logout = () => {
    const auth = useAuth();
    auth.logout();
    return <Redirect to='/account/login' />
}