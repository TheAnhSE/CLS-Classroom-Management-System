import React, { useState } from 'react';
import { InputField } from '../../../shared/components/InputField';
import { Button } from '../../../shared/components/Button';
import { useLogin } from '../hooks/useAuthQueries';

export const LoginForm: React.FC = () => {
  const { login, isLoading, error } = useLogin();
  const [email, setEmail] = useState('admin@cls.edu.vn'); // Default for mockup
  const [password, setPassword] = useState('123456');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    login({ email, password });
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col h-full justify-center">
      <div className="mb-9">
        <div className="inline-block bg-blue-50 text-blue-700 text-xs font-semibold px-3.5 py-1.5 rounded-full mb-4 tracking-wide">
          🔐 Đăng nhập hệ thống
        </div>
        <h2 className="text-3xl font-bold text-slate-900 mb-2">Chào mừng trở lại</h2>
        <p className="text-sm text-slate-500">Nhập thông tin tài khoản để tiếp tục</p>
      </div>

      {error && (
        <div className="inline-flex items-center gap-2 bg-red-50 text-red-700 text-xs px-3 py-2 rounded-lg mb-4 border border-red-200">
          <svg className="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          {error}
        </div>
      )}

      {/* Role Chips (Mockup visual only) */}
      <div className="flex gap-2 mb-6">
        <div className="flex-1 py-2 px-2 border-1.5 border-blue-600 bg-blue-50 text-blue-700 rounded-lg text-center text-xs font-medium cursor-pointer">
          👤 Academic Admin
        </div>
        <div className="flex-1 py-2 px-2 border-1.5 border-slate-200 text-slate-500 rounded-lg text-center text-xs font-medium cursor-pointer">
          📚 Teacher
        </div>
        <div className="flex-1 py-2 px-2 border-1.5 border-slate-200 text-slate-500 rounded-lg text-center text-xs font-medium cursor-pointer">
          📊 Director
        </div>
      </div>

      <div className="space-y-5">
        <InputField
          label="Email"
          type="email"
          placeholder="admin@cls.edu.vn"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        
        <div>
          <InputField
            label="Mật khẩu"
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <div className="text-right mt-2">
            <a href="#" className="text-sm text-blue-600 font-medium hover:underline">
              Quên mật khẩu?
            </a>
          </div>
        </div>
      </div>

      <Button
        type="submit"
        isLoading={isLoading}
        className="w-full mt-6 py-3.5 bg-gradient-to-br from-blue-600 to-blue-800 text-white shadow-[0_4px_15px_rgba(37,99,235,0.4)] hover:shadow-[0_6px_20px_rgba(37,99,235,0.6)] rounded-xl text-base"
      >
        Đăng nhập →
      </Button>

      <div className="mt-6 text-center text-xs text-slate-400 flex items-center justify-center gap-3">
        <span className="h-px bg-slate-200 flex-1"></span>
        <span>Hệ thống quản lý nội bộ</span>
        <span className="h-px bg-slate-200 flex-1"></span>
      </div>
    </form>
  );
};
