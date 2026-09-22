import { useEffect, useState } from 'react';
import './CommentsList.css';

async function getComments(postId) {
    const response = await fetch(`https://jsonplaceholder.typicode.com/comments?postId=${postId}`);
    if (!response.ok) {
        throw new Error(`Comments: ${response.status} - ${response.statusText}`);
    }
    return response.json();
}

export default function CommentsList({ post }) {
    const [isShowing, setIsShowing] = useState(false);
    const [comments, setComments] = useState([]);
    const [isLoaded, setIsLoaded] = useState(false);

    useEffect(() => {
        if (!isShowing) return;
        if (isLoaded) return;

        async function load() {
            try {
                const commentsData = await getComments(post.id);
                setComments(commentsData);
                setIsLoaded(true);
            } catch (e) {
                console.error('Failed to load comments:', e);
            }
        };

        load();
    }, [isShowing, post.id]);

    const handleBtnClick = () => {
        setIsShowing(prev => !prev);
    };

    return (
        <>
        {isShowing && (
            <div className="commentsContainer" style={isShowing ? {display: 'block'} : {display: 'none'}}>
                {comments.map(comment => (
                    <div key={comment.id} className="comment">
                        <h3>{comment.name}</h3>
                        <p>{comment.body}</p>
                        <h4 className="email">{comment.email}</h4>
                    </div>
                ))}
            </div>
        )}
            <button className="commentsBtn" onClick={handleBtnClick}>
                {isShowing ? 'Hide Comments' : 'Show Comments'}
            </button>
        </>
    )
}
