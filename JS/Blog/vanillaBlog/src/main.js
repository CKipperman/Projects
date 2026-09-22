import { showUserList } from './page';
import { loadUsers } from './users';

const homeBtn = document.querySelector('.homeBtn');

homeBtn.addEventListener('click', () => {
  showUserList();
});

loadUsers();