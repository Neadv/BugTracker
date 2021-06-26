import { useState } from "react";
import { api } from "../../api/api";
import { Link } from "react-router-dom";

export const PrivateComponent = () => {
    const [message, setMessage] = useState(null);
    const authorizedEndpoint = () => {
        api.get('home').then(res => setMessage(res.data.message)).catch(e => setMessage('error'));
    };

    return (
        <>
            <h1>PrivateComponent</h1>
            <Link className="btn btn-primary" to='/account/logout'>Logout</Link>
            <button className="btn btn-primary" onClick={authorizedEndpoint}>Authorized Endpoint</button>
            <div>{message}</div>
        </>
    );
}