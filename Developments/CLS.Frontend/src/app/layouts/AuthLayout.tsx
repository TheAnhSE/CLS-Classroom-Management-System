import React from 'react';
import { Outlet } from 'react-router-dom';

export const AuthLayout: React.FC = () => {
  // LoginPage contains the full-screen design, so AuthLayout just renders Outlet
  return (
    <Outlet />
  );
};
