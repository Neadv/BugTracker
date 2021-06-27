import { NavLink } from 'react-router-dom';
import './Sidebar.scss';

export const Sidebar = ({ role, isHide = false }) => {
    return (
        <nav className={`sidebar shadow ${isHide ? "hidden" : ""}`}>
            <ul>
                <li><NavLink activeClassName="active" exact to='/'><i className="bi bi-grid-1x2"></i>Dashboard</NavLink></li>
                <li><NavLink activeClassName="active" to='/projects'><i className="bi bi-kanban"></i>Projects</NavLink></li>
                <li><NavLink activeClassName="active" to='/tickets'><i className="bi bi-justify"></i>Tickets</NavLink></li>
                <li><NavLink activeClassName="active" to='/users'><i className="bi bi-people"></i>Manage Users</NavLink></li>
                <li><NavLink activeClassName="active" to='/roles'><i className="bi bi-person-plus"></i>Manage Role Assigment</NavLink></li>
                <li><NavLink activeClassName="active" to='/profile'><i className="bi bi-person"></i>Profile</NavLink></li>
            </ul>
        </nav>
    );
}