import express from 'express';
import http from 'http';
import path from 'path';
import { Server } from 'socket.io';

const app = express();
const server = http.createServer(app);
const io = new Server(server);

const __dirname = import.meta.dirname;
app.use(express.static(path.join(__dirname, 'public')));

io.on('connection', socket => {
  console.log('got a connection');

  socket.on('join', username => {
    socket.username = username.trim();
    io.emit('msg', {text: `${socket.username} joined the chat`});
  }); 

  socket.on('msg', data => {
    io.emit('msg', {type: 'chat', name: data.name, text: data.text});
  });

  socket.on('disconnect', () => {
    const user = socket.username ?? 'Someone'
    io.emit('msg', { text: `${user} disconnected` });
  });
});

server.listen(80);
