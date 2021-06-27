import { useAuth } from "../account/authHook";
import { Sidebar } from "./Sidebar";
import { Link } from "react-router-dom";
import { NavDropdown } from "react-bootstrap";
import "./Layout.scss";
import { useState } from "react";

export const Layout = ({ children }) => {
  const auth = useAuth();
  const [sidebarHidden, setHidden] = useState(false);

  const toggle = () => {
    setHidden(!sidebarHidden);
  }

  return (
    <>
      <Sidebar role={auth.user.role} isHide={sidebarHidden} />

      <div className="header">
        <div className="header-title">
          <button className="toggle-sidebar" onClick={toggle} style={!sidebarHidden ? {visibility: "hidden"} : null}><i className="bi bi-list"></i></button>
          <span><i className="bi bi-bug"></i>Bug Tracker</span>
          <button className="toggle-sidebar" onClick={toggle} style={sidebarHidden ? {visibility: "hidden"} : null}><i className="bi bi-x-lg"></i></button>
        </div>
        <div className="header-user">Logged in as: <span>{auth.user.username}</span></div>
        <NavDropdown title={auth.user.username} id="navbarScrollingDropdown">
          <Link to="/profile" className="dropdown-item">Profile</Link>
          <NavDropdown.Divider />
          <Link to="/account/logout" className="dropdown-item">Logout</Link>
        </NavDropdown>
      </div>

      <div className={`wrapper ${sidebarHidden ? "full" : ""}`}>
        {children}
      </div>
    </>
  );
}