import './css/NewQuoteBtn.css';

interface NewQuoteBtnProps {
    handleClick: () => void;
}

export default function NewQuoteBtn({handleClick}: NewQuoteBtnProps) {
    
    return (
        <button type='button' onClick={handleClick}>New Random Quote</button>
    )
}