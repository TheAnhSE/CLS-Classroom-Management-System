import React from 'react';
import { Outlet, Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../features/auth/authStore';

export const MainLayout: React.FC = () => {
  const logout = useAuthStore((state) => state.logout);
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const navItems = [
    { name: 'Dashboard', icon: '📊', path: '/' },
    { name: 'Học viên', icon: '👥', path: '/learners' },
    { name: 'Gói học phí', icon: '📦', path: '/packages' },
    { name: 'Lịch học', icon: '📅', path: '/sessions' },
    { name: 'Điểm danh', icon: '✅', path: '/attendance' },
    { name: 'Feedback', icon: '📝', path: '/feedback' },
  ];

  return (
    <div className="flex min-h-screen bg-slate-100 font-sans">
      {/* Sidebar */}
      <aside className="w-[240px] bg-slate-900 min-h-screen py-6 shrink-0 flex flex-col">
        <div className="px-5 pb-6 border-b border-white/10">
          <div className="text-white text-[17px] font-bold">🏫 CLS System</div>
          <div className="text-slate-400 text-[11px] mt-0.5">Academic Admin Portal</div>
        </div>
        
        <div className="flex-1 overflow-y-auto">
          <div className="px-3 pt-4 pb-2 text-[10px] text-slate-500 tracking-widest uppercase font-semibold">
            Main
          </div>
          <nav className="flex flex-col gap-0.5">
            {navItems.map((item) => {
              const isActive = location.pathname === item.path;
              return (
                <Link
                  key={item.path}
                  to={item.path}
                  className={`flex items-center gap-2.5 px-4 py-2.5 mx-2 rounded-lg text-[13px] transition-colors ${
                    isActive
                      ? 'bg-blue-500/15 text-blue-400 font-semibold'
                      : 'text-slate-400 hover:bg-slate-800 hover:text-slate-300'
                  }`}
                >
                  <span className="text-base">{item.icon}</span>
                  {item.name}
                </Link>
              );
            })}
          </nav>

          <div className="px-3 pt-6 pb-2 text-[10px] text-slate-500 tracking-widest uppercase font-semibold">
            Admin
          </div>
          <nav className="flex flex-col gap-0.5">
            <button
              onClick={handleLogout}
              className="flex items-center gap-2.5 px-4 py-2.5 mx-2 rounded-lg text-[13px] text-red-400 hover:bg-red-500/10 transition-colors text-left"
            >
              <span className="text-base">🚪</span>
              Đăng xuất
            </button>
          </nav>
        </div>
      </aside>

      {/* Main Content */}
      <main className="flex-1 flex flex-col overflow-hidden">
        {/* Topbar */}
        <header className="bg-white px-7 h-16 flex items-center justify-between border-b border-slate-200 shrink-0">
          <h1 className="text-lg font-bold text-slate-900">
            {navItems.find((i) => i.path === location.pathname)?.name || 'Dashboard Tổng quan'}
          </h1>
          <div className="flex items-center gap-3">
            <button className="text-xl hover:bg-slate-100 p-1.5 rounded-full transition-colors">
              🔔
            </button>
            <div className="w-9 h-9 rounded-lg bg-gradient-to-br from-blue-600 to-blue-400 flex items-center justify-center text-white text-xs font-bold">
              AA
            </div>
            <span className="text-[13px] text-slate-700 font-medium">Nguyễn Giáo Vụ</span>
          </div>
        </header>

        {/* Page Content */}
        <div className="flex-1 overflow-auto p-7">
          <Outlet />
        </div>
      </main>
    </div>
  );
};
