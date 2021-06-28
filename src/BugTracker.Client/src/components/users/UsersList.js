import { Button, Table } from "react-bootstrap";
import { Link } from "react-router-dom";

export const UsersList = ({users, onDelete, isActive, onActive}) => (
    <Table striped bordered size="sm">
        <thead>
            <tr>
                <th>Username</th>
                <th>Email</th>
                {isActive && <th>Roles</th>}
                <th></th>
            </tr>
        </thead>
        <tbody>
            {users.map(user => (
                <tr key={user.username}>
                    <td>{user.username}</td>
                    <td>{user.email}</td>
                    {isActive && <td>{user.roles.join(', ')}</td>}
                    <td>
                        <Link to={`/users/${user.username}`} className="btn btn-primary btn-sm mr-1">View</Link>
                        <Button variant="danger" size="sm" onClick={() => onDelete(user)}>Delete</Button>
                        { !isActive && <Button variant="warning" size="sm" className="ml-1" onCLick={() => onActive(user)}>Activate</Button> }
                    </td>
                </tr>
            ))}
        </tbody>
    </Table>
)