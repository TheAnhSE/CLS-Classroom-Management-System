import React from 'react';
import { LoginForm } from '../components/LoginForm';

export const LoginPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-slate-900 flex items-center justify-center relative overflow-hidden">
      {/* Background Shapes */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute w-[600px] h-[600px] rounded-full opacity-10 bg-[radial-gradient(circle,#3b82f6,transparent)] -top-[200px] -left-[200px]"></div>
        <div className="absolute w-[400px] h-[400px] rounded-full opacity-10 bg-[radial-gradient(circle,#60a5fa,transparent)] -bottom-[100px] -right-[100px]"></div>
        <div className="absolute w-[300px] h-[300px] rounded-full opacity-10 bg-[radial-gradient(circle,#2563eb,transparent)] top-1/2 left-[60%]"></div>
      </div>

      {/* Main Container */}
      <div className="w-[1100px] min-h-[640px] rounded-3xl overflow-hidden shadow-[0_40px_80px_rgba(0,0,0,0.5)] flex relative z-10 m-6">
        
        {/* Left Panel - Branding */}
        <div className="flex-[1.2] bg-gradient-to-br from-blue-700 via-blue-800 to-slate-900 p-[60px_50px] flex flex-col justify-between">
          <div className="flex items-center gap-3.5">
            <div className="w-11 h-11 bg-gradient-to-br from-blue-500 to-blue-400 rounded-xl flex items-center justify-center">
              <svg viewBox="0 0 24 24" className="w-6 h-6 fill-white">
                <path d="M12 3L1 9l11 6 9-4.91V17h2V9L12 3zM5 13.18v4L12 21l7-3.82v-4L12 17l-7-3.82z"/>
              </svg>
            </div>
            <div>
              <div className="text-xl font-bold text-white tracking-tight leading-tight">CLS System</div>
              <div className="text-[11px] text-blue-300 uppercase tracking-widest mt-0.5">Classroom Management</div>
            </div>
          </div>

          <div className="flex-1 flex flex-col justify-center py-10">
            <h1 className="text-[38px] font-bold text-white leading-[1.2] mb-4">
              Quản lý trung tâm<br/>thông minh & <span className="text-blue-400">hiệu quả</span>
            </h1>
            <p className="text-[15px] text-slate-400 leading-[1.7] max-w-[360px]">
              Hệ thống số hóa toàn diện quy trình học vụ — từ điểm danh, lịch học đến thông báo phụ huynh tự động.
            </p>
          </div>

          <div className="flex gap-8">
            <div>
              <div className="text-[28px] font-bold text-blue-400">150+</div>
              <div className="text-xs text-slate-400 mt-1">Học viên</div>
            </div>
            <div>
              <div className="text-[28px] font-bold text-blue-400">100%</div>
              <div className="text-xs text-slate-400 mt-1">Tự động hóa</div>
            </div>
            <div>
              <div className="text-[28px] font-bold text-blue-400">12h</div>
              <div className="text-xs text-slate-400 mt-1">SLA Feedback</div>
            </div>
          </div>
        </div>

        {/* Right Panel - Form */}
        <div className="flex-1 bg-white p-[56px_48px]">
          <LoginForm />
        </div>
      </div>
    </div>
  );
};
