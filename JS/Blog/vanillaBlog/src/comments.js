import './css/comments.css';
import { load } from './utils.js';

function createCommentElement(comment) {
    const { 
        name, 
        email, 
        body 
    } = comment;

    const div = document.createElement('div');
    div.className = 'comment';
    div.innerHTML = `
        <h3>${name}</h3>
        <p>${body}</p>
        <h4 class="email">${email}</h4>
    `;
    return div;
}

export function commentsToggle(postCard, postId) {
    const btn = postCard.querySelector('.commentsBtn');
    const container = postCard.querySelector('.commentsContainer');

    if (!btn || !container) return;

    let isLoaded = false;
    let isShowing = false;

    btn.addEventListener('click', async () => {
        isShowing = !isShowing;

        if (isShowing) {
            btn.textContent = 'Hide Comments';
            container.style.display = 'block';

            if (!isLoaded) {
                container.innerHTML = '<p class="loading">Loading comments...</p>';

                try {
                    const comments = await load(
                        `https://jsonplaceholder.typicode.com/comments?postId=${postId}`,
                        createCommentElement
                    );

                    container.innerHTML = ''; // clear loading message
                    comments.forEach(comment => container.appendChild(comment));
                    isLoaded = true;
                } catch (e) {
                    container.innerHTML = `<p class="error">Failed to load comments: ${e.message}</p>`;
                }
            }
        } else {
            btn.textContent = 'Show Comments';
            container.style.display = 'none';
        }
    });
}