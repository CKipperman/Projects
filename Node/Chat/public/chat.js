const socketIo = io();

const loginForm = document.querySelector('#loginForm');
const login = document.querySelector('#login');
const chat = document.querySelector('#chat');
const welcome = document.querySelector('#welcome');

let username;

loginForm.addEventListener('submit', e => {
  e.preventDefault();
  username = document.querySelector('#usernameInput').value.trim();

  if (username) {
    login.style.display = 'none';
    welcome.innerHTML = `Welcome ${username}`;
    chat.style.display = 'block';

    socketIo.emit('join', username);
  }
});

const messages = document.querySelector('#messages');
socketIo.on('msg', (data) => {
  if (data.type === 'chat') {
    messages.innerHTML += `<div>${data.name} said - ${data.text}</div>`;
  } else {
    messages.innerHTML += `<h5 id="join">${data.text}</h5>`
  }
});

const messageInput = document.querySelector('#messageInput');

document.querySelector('#messageForm').addEventListener('submit', e => {
  e.preventDefault();
  const text = messageInput.value;
  if (text) {
    socketIo.emit('msg', { name: username, text: text });
    messageInput.value = '';
  }
});
