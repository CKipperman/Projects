# PCS Contacts (MySQL Version)

A full CRUD contact management application built with Node.js, Express, Hogan.js, and MySQL.

## About the Project

Users can view, add, edit, and delete contacts. All data is stored in a MySQL database using a connection pool. The app uses server-side rendering with Hogan.js templates.

## Technologies Used

- Node.js (ES Modules)
- Express
- MySQL (mysql2)
- Hogan.js (hjs) – server-side templating
- dotenv / environment variables (for database credentials)

## Features Practiced

- Full CRUD operations with a real database
- MySQL connection pooling
- Parameterized queries (prevents SQL injection)
- Express routing and middleware
- Server-side rendering with templates and partials
- Handling form data
- Basic error handling
- Using environment variables for sensitive data

## Setup

1. Create a MySQL database and a `contacts` table.
2. Set environment variables:
   ```bash
   SQL_USER=your_username
   SQL_PWD=your_password
3. Install dependencies:
   ```bash
   npm install
4. Start the server:
   ```bash
   npm start
5. Open:
   http://localhost:3000

## Purpose

This project was created to practice building a database-backed CRUD application with Express and MySQL.

---

Created by: Chavie Kipperman  
Year: 2026