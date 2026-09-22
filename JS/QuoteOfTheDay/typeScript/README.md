# Motivational Quote of the Day (React + TypeScript)

A React application written in TypeScript that displays a motivational quote of the day and allows users to generate random quotes.

## About the Project

The app shows a daily quote based on the current day of the year, along with the current date. Users can also click a button to get a new random quote. Quotes are stored in a local JSON file. This version adds TypeScript for type safety.

## Technologies Used

- React
- TypeScript
- dayjs (for date handling)
- JSON
- CSS

## Features Practiced

- React functional components with TypeScript
- TypeScript interfaces for props
- State management with `useState`
- Working with external JSON data
- Date formatting with the `dayjs` library
- Component composition
- Generating a consistent “quote of the day” based on the day of the year

## Components

- `App.tsx` – Main component and state management
- `Date.tsx` – Displays the current date
- `QuoteFormat.tsx` – Displays the quote and author (with typed props)
- `NewQuoteBtn.tsx` – Button to load a random quote (with typed props)
- `Quotes.tsx` – Helper functions to get the daily or random quote

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the development server:
   npm run dev

## Purpose

This project was created to practice React with TypeScript, including typing component props and working with dates and external data.

---

Created by: Chavie Kipperman  
Year: 2025-2026