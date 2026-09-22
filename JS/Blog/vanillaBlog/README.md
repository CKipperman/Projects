# Blog Site (Vanilla JS + Vite)

A blog-style web application built with vanilla JavaScript and Vite. It displays users, their posts, and comments using data from the JSONPlaceholder API.

## About the Project

Users can browse a list of authors, click on a user to view their posts, and expand any post to load and view its comments. The app is built without frameworks, using modular JavaScript and the Fetch API.

## Technologies Used

- Vanilla JavaScript (ES Modules)
- Vite
- Fetch API
- CSS

## Features Practiced

- Fetching data from an external API
- Modular code organization (separate files for users, posts, comments, etc.)
- DOM manipulation (creating and updating elements)
- Event handling
- Show/hide functionality for comments
- Reusable utility functions
- Async/await error handling

## Project Structure

- `main.js` – Entry point
- `users.js` – Loads and displays the list of users
- `posts.js` – Loads and displays posts for a selected user
- `comments.js` – Handles loading and toggling comments
- `page.js` – Controls which section (users or posts) is visible
- `utils.js` – Shared helper functions (`load`, `getInitials`)

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the development server:
   npm run dev

## Purpose
This project was created to practice building a multi-view application with vanilla JavaScript, modular code structure, and working with external APIs — without using a front-end framework.

Created by: Chavie Kipperman
Year: 2025-2026