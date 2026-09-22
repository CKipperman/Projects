import './css/posts.css';
import { commentsToggle } from "./comments";
import { appendToPostSection } from "./page";
import { load } from "./utils";

function createPostCard(post) {
    const {
        title,
        body,
        id: postId
    } = post;

    const card = document.createElement('div');
    card.className = 'postCard';
    card.innerHTML = `
        <h3>${title}</h3>
        <p>${body}</p>
        <div class="commentsContainer" style="display:none;"></div>
        <button class="commentsBtn">Show Comments</button>
    `;

    commentsToggle(card, postId);
    return card;
}

export async function loadUserPosts(user) {
    try {
        const posts = await load(
            `https://jsonplaceholder.typicode.com/posts?userId=${user.id}`,
            createPostCard
        );

        posts.forEach(card => appendToPostSection(card));
    } catch (e) {
        appendToPostSection(
            document.createTextNode('Failed to load posts')
        );
    }
}