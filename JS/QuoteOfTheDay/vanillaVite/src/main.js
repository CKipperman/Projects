import './style.css'
import dayjs from 'dayjs'
import { getQuoteOfTheDay, getRandomQuote } from './quotes.js'

const date = document.querySelector('#date')
const quote = document.querySelector('#quote')
const author = document.querySelector('#author')
const newQuoteBtn = document.querySelector('#newQuote')

function displayQuote(quoteInfo) {
  quote.textContent = `“${quoteInfo.text}”`
  author.textContent = `— ${quoteInfo.author}`
}

date.textContent = dayjs().format('dddd, MMMM D, YYYY')
displayQuote(getQuoteOfTheDay())

newQuoteBtn.addEventListener('click', () => {
  displayQuote(getRandomQuote())
})
