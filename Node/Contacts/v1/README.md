# PCS Contacts

A simple contact management web application built with Node.js, Express, and Hogan.js (server-side rendering).

## About the Project

Users can view a list of contacts, add new contacts, edit existing ones, and delete contacts. The app uses server-side rendering with the Hogan.js templating engine and stores data in memory.

## Technologies Used

- Node.js
- Express
- Hogan.js (hjs) – server-side templating
- HTML forms
- CSS

## Features Practiced

- Full CRUD operations (Create, Read, Update, Delete)
- Express routing (GET and POST)
- Server-side rendering with templates and partials
- Handling form data with `express.urlencoded`
- Redirecting after form submissions
- Basic layout templates
- In-memory data storage

## Available Pages / Routes

| Route | Description |
|-------|-------------|
| `GET /` | View all contacts |
| `GET /addContact` | Show the add contact form |
| `POST /addContact` | Create a new contact |
| `GET /editContact/:id` | Show the edit form for a contact |
| `POST /editContact/:id` | Update a contact |
| `POST /deleteContact/:id` | Delete a contact |
| `GET /api/contacts` | Return contacts as JSON |

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   npm start
3. Open a browser and go to:
   http://localhost:3000

## Purpose

This project was created to practice building a full CRUD application with Express and server-side rendering using Hogan.js templates.

---

Created by: Chavie Kipperman  
Year: 2026