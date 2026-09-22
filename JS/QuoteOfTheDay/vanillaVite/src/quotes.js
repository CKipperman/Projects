import dayjs from 'dayjs'
import quotes from './quotes.json'

export function getQuoteOfTheDay() {
  const startOfYear = dayjs().startOf('year');
  const now = dayjs();
  const dayOfYear = now.diff(startOfYear, 'day');
  const index = dayOfYear % quotes.length;
  return quotes[index];
}

export function getRandomQuote() {
  const index = Math.floor(Math.random() * quotes.length);
  return quotes[index];
}