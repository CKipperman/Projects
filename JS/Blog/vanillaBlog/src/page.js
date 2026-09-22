const userSection = document.querySelector('.userSection');
const postSection = document.querySelector('.postSection');

export function showUserList() {
    userSection.style.display = 'block';
    postSection.style.display = 'none';
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

export function showUserPosts(user) {
    userSection.style.display = 'none';
    postSection.style.display = 'block';

    postSection.innerHTML = `<h2>${user.name}'s Posts:</h2>`;
}

export function appendToPostSection(element) {
    postSection.appendChild(element);
}