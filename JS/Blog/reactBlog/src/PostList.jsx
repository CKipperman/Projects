import { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import './PostList.css';
import CommentsList from './CommentsList';

export default function PostList({ users }) {
    const { name } = useParams();
    const [posts, setPosts] = useState([]);

    const selectedUser = users.find(
        u => u.name === name
    );

    useEffect(() => {
        if (!selectedUser?.id) return;
        (async function () {
            try {
                const response = await fetch(
                    `https://jsonplaceholder.typicode.com/posts?userId=${selectedUser.id}`
                );
                const posts = await response.json();
                setPosts(posts);
            } catch (e) {
                console.error('Failed to load posts:', e);
            }
        })();
    }, [selectedUser?.id]);

    if (!selectedUser) {
        return <div className="loading">Loading posts...</div>;
    }

    return (
        <div className="postSection">
            <h2>{selectedUser.name}'s Posts:</h2>
            {posts.map(post => (
                <div key={post.id} className="postCard" >
                    <h3>{post.title}</h3>
                    <p>{post.body}</p>
                    <CommentsList post={post} postId={post.id}/>
                </div>
            ))}
        </div>
    );
}
