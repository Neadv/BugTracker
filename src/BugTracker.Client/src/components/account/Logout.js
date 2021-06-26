import { useAuth } from "./authHook"
import { Redirect } from "react-router";
import { useEffect } from "react";

export const Logout = () => {
    const auth = useAuth();

    useEffect(() => {
        auth.logout();
    }, [auth])
    
    return auth.isAuthorized ? <Redirect to="/account/login" /> : <h1 className="text-center">Logout</h1>
}