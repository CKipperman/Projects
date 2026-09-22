# Blog Site – Users, Posts & Comments

A React application that displays users, their blog posts, and comments using data from the JSONPlaceholder API.

## About the Project

This project simulates a simple blog platform. Users can browse a list of authors, click on a user to view their posts, and expand each post to see its comments. It uses React Router for navigation and fetches live data from a public API.

## Technologies Used

- React
- React Router
- JavaScript (Fetch API / async-await)
- CSS

## Features Practiced

- Fetching data from an external API
- Managing state with `useState` and `useEffect`
- Client-side routing with React Router
- Passing data between components via props
- Conditional rendering (show/hide comments)
- Dynamic routes using URL parameters
- Reusable components

## Main Components

- `App.jsx` – Sets up routing and loads the list of users
- `Header.jsx` – Site header with logo and home link
- `UserList.jsx` – Displays all users in a card layout
- `PostList.jsx` – Shows posts for a selected user
- `CommentsList.jsx` – Loads and toggles comments for each post

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the development server:
   npm run dev

## Purpose

This project was created to practice working with external APIs, React Router, and component-based architecture in a multi-page style React application.

Created by: Chavie Kipperman
Year: 2025-2026