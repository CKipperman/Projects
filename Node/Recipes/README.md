# Recipes API

A RESTful API for managing recipes, built with Node.js, Express, and MySQL.

## About the Project

This API allows clients to create, read, update, and delete recipes stored in a MySQL database. It follows standard REST conventions and returns JSON responses.

## Technologies Used

- Node.js (ES Modules)
- Express
- MySQL (mysql2)
- Connection pooling

## Features Practiced

- Building a RESTful API
- Full CRUD operations with MySQL
- Parameterized queries (SQL injection safe)
- Proper HTTP status codes (200, 201, 204, 404)
- Using a MySQL connection pool
- Environment variables for database credentials
- Express Router for organizing routes

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/recipes-api` | Get all recipes |
| `POST` | `/recipes-api` | Create a new recipe |
| `GET` | `/recipes-api/:id` | Get a single recipe by ID |
| `PUT` | `/recipes-api/:id` | Update a recipe |
| `DELETE` | `/recipes-api/:id` | Delete a recipe |

## Setup

1. Create a MySQL database and a `recipes` table (with at least `id` and `name` columns).
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
5. The API will be available at:
   http://localhost:3000/recipes-api

## Purpose

This project was created to practice building a clean REST API with Express and MySQL, including proper status codes and database interaction.

---

Created by: Chavie Kipperman  
Year: 2026