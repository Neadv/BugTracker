import { useState } from "react";
import { api } from "../../api/api";
import { useAuth } from "../account/authHook";

export const PrivateComponent = () => {
    const [message, setMessage] = useState(null);
    const authorizedEndpoint = () => {
        api.get('home').then(res => setMessage(res.data.message)).catch(e => setMessage('error'));
    };

    const auth = useAuth();
    return (
        <>
            <h1>PrivateComponent</h1>
            <button className="btn btn-primary" onClick={() => auth.logout()}>Logout</button>
            <button className="btn btn-primary" onClick={authorizedEndpoint}>Authorized Endpoint</button>
            <div>{message}</div>
        </>
    );
}