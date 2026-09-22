import './css/Quote.css';

interface QuoteFormatProps {
    quoteInfo: {
        text: string,
        author?: string
    }
}

export default function QuoteFormat({ quoteInfo: { text, author = 'Anonymous'}, }: QuoteFormatProps ) {
    return (
        <>
            <blockquote>“{text}”</blockquote>
            <p id="author">— {author}</p>
        </>
    )
}
