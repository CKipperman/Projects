import { useState } from 'react';
import './css/App.css'
import Date from './Date.tsx';
import { getQuoteOfTheDay, getRandomQuote } from './Quotes.tsx';
import QuoteFormat from './QuoteFormat.tsx';
import NewQuoteBtn from './NewQuoteBtn.tsx';

function App() {
  const [currentQuote, setCurrentQuote] = useState(getQuoteOfTheDay());

  const handleNewQuote = () => {
    setCurrentQuote(getRandomQuote());
  };

  return (
    <>
      <h1>Motivational Quote of the Day</h1>
      <Date />
      <QuoteFormat quoteInfo={currentQuote}/>
      <NewQuoteBtn handleClick={handleNewQuote}/>
    </>
  )
}

export default App
