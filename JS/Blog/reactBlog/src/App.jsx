import './App.css';
import { BrowserRouter, Navigate, Outlet, Route, Routes } from 'react-router';
import Header from './Header';
import { useEffect, useState } from 'react';
import UserList from './UserList';
import PostList from './PostList';

export default function App() {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    (async function () {
      try {
        const response = await fetch('https://jsonplaceholder.typicode.com/users');
        if (!response.ok) {
          throw new Error(`Users: ${response.status} - ${response.statusText}`);
        }
        const users = await response.json();
        setUsers(users);
      } catch (e) {
        console.error('Failed to initialize app:', e);
      }
    }());
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={
          <>
            <Header />
            <Outlet />
          </>
        }>
          <Route index element={<UserList users={users} />} />
          <Route path='/posts/:name' element={<PostList users={users} />} />
          <Route path='*' element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
