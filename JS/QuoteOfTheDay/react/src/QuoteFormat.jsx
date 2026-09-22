import './css/Quote.css';

export default function QuoteFormat({ quoteInfo }) {
    return (
        <>
            <blockquote>“{quoteInfo.text}”</blockquote>
            <p id="author">— {quoteInfo.author}</p>
        </>
    )
}
