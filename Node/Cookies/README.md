# Cookies Demo

A simple Express application that demonstrates how to use signed cookies to remember a user’s name and track page visits.

## About the Project

When a user visits the site, the app counts how many times they have visited the home page. Users can also enter their name, which is stored in a signed cookie so the site can greet them on future visits.

## Technologies Used

- Node.js
- Express
- cookie-parser (with signed cookies)
- Hogan.js (hjs) – server-side templating

## Features Practiced

- Setting and reading signed cookies
- Tracking visit counts with cookies
- Remembering a username across visits
- Using middleware to handle cookie logic
- Server-side rendering with Hogan.js
- Conditional rendering based on cookie data

## How It Works

1. Every visit to the home page increases a `visits` cookie counter.
2. A user can enter their name, which is saved in a signed `username` cookie.
3. On future visits, the app greets the user by name and shows the total number of visits.

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   ```bash
   npm start
3. Open:
   http://localhost:3000

## Purpose

This project was created to practice working with cookies in Express, including signed cookies, middleware, and persisting simple user data between requests.

---

Created by: Chavie Kipperman  
Year: 2026