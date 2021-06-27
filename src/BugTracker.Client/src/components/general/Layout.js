import { useAuth } from "../account/authHook";
import { Sidebar } from "./Sidebar";
import { Link } from "react-router-dom";
import { NavDropdown } from "react-bootstrap";
import "./Layout.scss";

export const Layout = ({ children }) => {
    const auth = useAuth();
    return (
        <div className="d-flex">
            <Sidebar role={auth.user.role} />
            <div className="wrapper">
                <header className="header">
                    <div className="header-user">Logged in as: <span>{auth.user.username}</span></div>
                    <NavDropdown title={auth.user.username} id="navbarScrollingDropdown">   
                        <Link to="/profile" className="dropdown-item">Profile</Link>
                        <NavDropdown.Divider />
                        <Link to="/account/logout" className="dropdown-item">Logout</Link>
                    </NavDropdown>
                </header>
                <div className="p-3">
                    {children}
                </div>
            </div>
        </div>
    );
}