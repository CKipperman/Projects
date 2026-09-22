import './css/users.css';
import { showUserPosts } from './page';
import { loadUserPosts } from './posts';
import { getInitials, load } from "./utils";

const userGrid = document.querySelector('.userGrid');

function createUserCard(user) {
    const {
        name,
        username,
        website,
        company: { name: companyName, catchPhrase },
    } = user;

    const userCard = document.createElement('div');
    userCard.className = 'userCard';
    userCard.innerHTML = `<div class='initial'>${getInitials(name)}</div>
                                <div class='content'>
                                    <h3 id='userName'>${name}</h3>
                                    <div class='info'>
                                        <div class='userInfo'>
                                            <h4>@${username}</h4>
                                            <div>${website}</div>
                                        </div>
                                        <div class='companyInfo'>
                                            <h3>${companyName}</h3>
                                            <h4>"${catchPhrase}"</h4>
                                        </div>
                                    </div>
                                </div>`

    userCard.addEventListener('click', () => {
        showUserPosts(user);
        loadUserPosts(user);           
    });

    return userCard;
}

export async function loadUsers() {
    userGrid.innerHTML = '';
    try {
        const users = await load(
            'https://jsonplaceholder.typicode.com/users',
            createUserCard
        );

        users.forEach(card => userGrid.appendChild(card));
    } catch (e) {
        userGrid.innerHTML = `<div class="error">Failed to load users</div>`;
    }
}