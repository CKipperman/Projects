import { useState } from 'react';
import './css/App.css'
import Date from './Date.jsx';
import NewQuoteBtn from './NewQuoteBtn.jsx';
import QuoteFormat from './QuoteFormat.jsx';
import { getQuoteOfTheDay, getRandomQuote } from './Quotes.jsx';

function App() {
  const [currentQuote, setCurrentQuote] = useState(getQuoteOfTheDay());

  const handleNewQuote = () => {
    setCurrentQuote(getRandomQuote());
  };

  return (
    <>
      <h1>Motivational Quote of the Day</h1>
      <Date />
      <QuoteFormat quoteInfo={currentQuote} />
      <NewQuoteBtn handleClick={handleNewQuote} />
    </>
  )
}

export default App
