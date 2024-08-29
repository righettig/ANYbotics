'use client';

import { useState } from 'react';

import Login from './features/login/login';
import Sidebar from './features/layout/sidebar';
import Events from './features/events/components/events';
import Users from './features/users/users';

const Home = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [selectedView, setSelectedView] = useState<'events' | 'users'>('events');
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
  };

  const handleSelectView = (view: 'events' | 'users') => {
    setSelectedView(view);
  };

  const handleSidebarToggle = () => {
    setIsSidebarCollapsed(!isSidebarCollapsed);
  };

  if (!isLoggedIn) {
    return <Login onLogin={handleLogin} />;
  }

  return (
    <div style={{ display: 'flex' }}>
      <Sidebar
        onLogout={handleLogout}
        onSelect={handleSelectView}
        onToggle={handleSidebarToggle}
        isCollapsed={isSidebarCollapsed}
      />
      <div
        style={{
          marginLeft: isSidebarCollapsed ? '0px' : '180px',
          padding: '20px 20px 20px 40px',
          width: `calc(100% - ${isSidebarCollapsed ? '60px' : '220px'})`,
          transition: 'width 0.3s ease, margin-left 0.3s ease',
        }}
      >
        {selectedView === 'events' ? <Events /> : <Users />}
      </div>
    </div>
  );
};

export default Home;
