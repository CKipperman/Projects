# Sessions Demo

A simple Express application that demonstrates how to use server-side sessions to remember a user’s name and track page visits.

## About the Project

Users can enter their name, which is stored in a session. The app greets them by name on every page and keeps a visit counter that increases each time they visit the “View Counter” page.

## Technologies Used

- Node.js
- Express
- express-session
- Hogan.js (hjs) – server-side templating

## Features Practiced

- Setting up and using Express sessions
- Storing user data (name) in a session
- Tracking a visit count with sessions
- Sharing session data across multiple pages
- Conditional rendering (show name form only if no name is stored)
- Basic navigation between pages

## How It Works

1. On the home page, users can enter their name.
2. The name is saved in the session and displayed on all pages.
3. Visiting the “View Counter” page increases a session-based visit count.
4. The “About Us” page also shows the currently logged-in name.

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   ```bash
   npm start
3. Open a browser and go to:
   http://localhost:3000

## Purpose

This project was created to practice working with Express sessions for storing user-specific data across multiple requests.

---

Created by: Chavie Kipperman  
Year: 2026