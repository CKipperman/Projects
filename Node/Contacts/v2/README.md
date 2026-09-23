# PCS Contacts (In-Memory Version 2)

A simple contact management web application built with Node.js, Express, and Hogan.js using in-memory storage.

## About the Project

Users can view a list of contacts, add new contacts, edit existing ones, and delete contacts. Data is stored in a JavaScript array (it resets when the server restarts). This version also includes basic JSON API endpoints.

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
- Handling form data
- Redirecting after form submissions
- Basic JSON API endpoints (`/contacts` and `/contacts/:id`)
- In-memory data storage

## Available Routes

| Route | Description |
|-------|-------------|
| `GET /` | View all contacts |
| `GET /contacts` | Return all contacts as JSON |
| `GET /contacts/:id` | Return a single contact as JSON |
| `GET /addContact` | Show the add contact form |
| `POST /addContact` | Create a new contact |
| `GET /editContact/:id` | Show the edit form |
| `POST /editContact/:id` | Update a contact |
| `POST /deleteContact/:id` | Delete a contact |

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   npm start
3. Open a browser and go to:
   http://localhost:3000

## Purpose

This project was created to practice building a CRUD application with Express and server-side rendering, using in-memory storage as a stepping stone toward a database version.

---

Created by: Chavie Kipperman  
Year: 2026