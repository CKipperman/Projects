# Real-Time Chat App

A simple real-time chat application built with Node.js, Express, and Socket.IO.

## About the Project

Users enter a username to join the chat. Once connected, they can send messages that appear instantly for everyone in the room. The app also shows notifications when someone joins or leaves.

## Technologies Used

- Node.js
- Express
- Socket.IO
- HTML / CSS / Vanilla JavaScript

## Features Practiced

- Setting up a real-time server with Socket.IO
- Handling client connections and disconnections
- Emitting and listening for custom events (`join`, `msg`)
- Broadcasting messages to all connected clients
- Serving static files with Express
- Building a simple chat interface

## How It Works

1. User enters a name and joins the chat
2. A “joined” message is broadcast to everyone
3. Users can type and send messages in real time
4. When a user leaves, a “disconnected” message is shown

## How to Run

1. Install dependencies:
   ```bash
   npm install
2. Start the server:
   npm start
3. Open a browser and go to:text
   http://localhost:80
4. Open multiple tabs or browsers to test chatting between users.

## Purpose

This project was created to practice real-time communication with WebSockets (via Socket.IO) and building a basic full-stack chat application.

---

Created by: Chavie Kipperman  
Year: 2026