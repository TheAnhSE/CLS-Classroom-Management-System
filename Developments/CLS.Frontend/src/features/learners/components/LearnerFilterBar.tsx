import React from 'react';

export const LearnerFilterBar: React.FC = () => {
  return (
    <div className="flex items-center gap-3 mb-5 flex-wrap">
      <div className="relative flex-1 min-w-[260px]">
        <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
          <svg className="w-4 h-4 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
        </div>
        <input 
          type="text" 
          className="w-full pl-9 pr-3 py-2.5 border-1.5 border-slate-200 rounded-xl text-[13px] text-slate-700 bg-white focus:outline-none focus:border-blue-500 transition-colors"
          placeholder="Tìm theo tên, email, mã học viên..." 
        />
      </div>
      
      <select className="px-3 py-2.5 border-1.5 border-slate-200 rounded-xl text-[13px] text-slate-700 bg-white focus:outline-none focus:border-blue-500 cursor-pointer outline-none">
        <option>Tất cả trạng thái</option>
        <option>Đang hoạt động</option>
        <option>Tạm dừng</option>
        <option>Đã rút</option>
      </select>
      
      <select className="px-3 py-2.5 border-1.5 border-slate-200 rounded-xl text-[13px] text-slate-700 bg-white focus:outline-none focus:border-blue-500 cursor-pointer outline-none">
        <option>Tất cả môn học</option>
        <option>Tiếng Anh</option>
        <option>Kids Coding</option>
      </select>
      
      <select className="px-3 py-2.5 border-1.5 border-slate-200 rounded-xl text-[13px] text-slate-700 bg-white focus:outline-none focus:border-blue-500 cursor-pointer outline-none">
        <option>Sắp xếp: Tên A-Z</option>
        <option>Tên Z-A</option>
        <option>Mới nhất</option>
        <option>Gói sắp hết</option>
      </select>
    </div>
  );
};
