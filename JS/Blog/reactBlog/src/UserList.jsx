import { useNavigate } from 'react-router';
import './UserList.css';

function getInitials(name) {
    return name.split(' ').map(word => word[0].toUpperCase()).slice(0, 2).join('');
}

export default function UserList({ users }) {
    const navigate = useNavigate();

    return (
        <div className="userSection">
            <h2>Our Blogs:</h2>
            <div className="userGrid">
                {users.map(user => (
                    <div key={user.id} className="userCard" onClick={() => { navigate(`/posts/${user.name}`);}}>
                        <div className='initial'>{getInitials(user.name)}</div>
                        <div className='content'>
                            <h3 id='userName'>{user.name}</h3>
                            <div className='info'>
                                <div className='userInfo'>
                                    <h4>@{user.username}</h4>
                                    <div>{user.website}</div>
                                </div>
                                <div className='companyInfo'>
                                    <h3>{user.company.name}</h3>
                                    <h4>"{user.company.catchPhrase}"</h4>
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}
