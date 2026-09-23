# Express Calculator API

A simple REST API built with Node.js and Express that performs basic arithmetic operations.

## About the Project

This API accepts numbers and an optional operator through URL parameters or query strings and returns the calculated result. It includes input validation and proper error handling for missing or invalid values.

## Technologies Used

- Node.js
- Express
- ES Modules

## Features Practiced

- Creating a basic Express server
- Handling different routes (`/add`, `/subtract`, `/operate`)
- Supporting both route parameters and query parameters
- Writing custom middleware for validation (`parseNumbers`, `parseOperator`)
- Error handling with appropriate HTTP status codes
- Preventing division by zero

## Available Endpoints

| Endpoint | Example | Description |
|----------|---------|-------------|
| `GET /add` | `/add?a=5&b=3` | Adds two numbers |
| `GET /add/:a/:b` | `/add/5/3` | Adds two numbers (route params) |
| `GET /subtract` | `/subtract?a=10&b=4` | Subtracts two numbers |
| `GET /subtract/:a/:b` | `/subtract/10/4` | Subtracts two numbers (route params) |
| `GET /operate` | `/operate?op=+&a=5&b=3` | Performs +, -, *, or / |
| `GET /operate/:op/:a/:b` | `/operate/+/5/3` | Performs operation with route params |

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   node express.js
3. Then open a browser or use a tool like Postman / curl and go to:
   http://localhost:3000/add?a=5&b=3

You can also try other endpoints, for example:
http://localhost:3000/subtract?a=10&b=4
http://localhost:3000/operate?op=*&a=6&b=7
http://localhost:3000/add/5/3

## Purpose

This project was created to practice building a basic API with Express, writing middleware, and handling user input validation on the server side.

---

Created by: Chavie Kipperman  
Year: 2026