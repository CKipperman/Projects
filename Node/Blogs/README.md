# Full-Stack Blog (React + Express + MongoDB + Socket.IO)

A real-time blog application with user authentication, posts, and comments. Built as a full-stack project with a React frontend and a Node.js/Express backend.

## About the Project

Users can register, log in, create blog posts, and add comments. New posts and comments appear in real time for all connected users thanks to Socket.IO. Data is stored in MongoDB, and passwords are securely hashed with bcrypt.

## Project Structure
blog/
├── blog-api/          # Backend (Express + MongoDB + Socket.IO)
└── blog-client/       # Frontend (React + Vite)


## Technologies Used

### Backend
- Node.js + Express
- MongoDB
- Socket.IO
- express-session
- bcrypt (password hashing)
- Joi (validation)

### Frontend
- React
- React Router
- Socket.IO Client
- Vite
- dayjs

## Features

- User registration and login
- Session-based authentication
- Create posts (authenticated users only)
- Add comments to posts
- Real-time updates for new posts and comments
- Input validation (frontend + backend)
- Password hashing
- Protected routes/middleware

## How to Run

### 1. Backend (`blog-api`)

1. Make sure MongoDB is running locally.
2. Install dependencies:
   ```bash
   cd blog-api
   npm install
3. Start the server:
   ```bash
   npm start
The API runs on http://localhost:8080

### 2. Frontend (`blog-client`)

1. Install dependencies:
   ```bash
   cd blog-client
   npm install
2. Start the development server:
   ```bash
   npm run dev
The app runs on http://localhost:5173

## Purpose

This project was created to practice building a full-stack real-time application with authentication, a NoSQL database, and WebSockets.

---

Created by: Chavie Kipperman  
Year: 2026