'use client';

import { useState } from 'react';

import Login from './features/login/login';
import Sidebar from './features/layout/sidebar';
import Events from './features/events/components/events';
import Users from './features/users/users';

const Home = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [selectedView, setSelectedView] = useState<'events' | 'users'>('events');

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
  };

  const handleSelectView = (view: 'events' | 'users') => {
    setSelectedView(view);
  };

  if (!isLoggedIn) {
    return <Login onLogin={handleLogin} />;
  }

  return (
    <div style={{ display: 'flex' }}>
      <Sidebar onLogout={handleLogout} onSelect={handleSelectView} />
      <div style={{ marginLeft: '220px', padding: '20px', width: 'calc(100% - 240px)' }}>
        {selectedView === 'events' ? <Events /> : <Users />}
      </div>
    </div>
  );
};

export default Home;
