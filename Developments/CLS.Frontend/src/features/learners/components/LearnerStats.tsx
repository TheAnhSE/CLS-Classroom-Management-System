import React from 'react';

interface StatsProps {
  total: number;
}

export const LearnerStats: React.FC<StatsProps> = ({ total }) => {
  return (
    <div className="flex gap-4 mb-5">
      <div className="bg-white rounded-xl px-4 py-2.5 flex items-center gap-2 shadow-sm border border-slate-100 text-[13px] font-medium text-slate-700">
        <span className="text-base">👥</span>
        <strong className="text-blue-600 text-sm">{total}</strong> Tổng học viên
      </div>
      <div className="bg-white rounded-xl px-4 py-2.5 flex items-center gap-2 shadow-sm border border-slate-100 text-[13px] font-medium text-slate-700">
        <span className="text-base">✅</span>
        <strong className="text-green-600 text-sm">{total > 0 ? total - 1 : 0}</strong> Đang hoạt động
      </div>
      <div className="bg-white rounded-xl px-4 py-2.5 flex items-center gap-2 shadow-sm border border-slate-100 text-[13px] font-medium text-slate-700">
        <span className="text-base">⏸</span>
        <strong className="text-slate-500 text-sm">1</strong> Tạm dừng
      </div>
      <div className="bg-white rounded-xl px-4 py-2.5 flex items-center gap-2 shadow-sm border border-slate-100 text-[13px] font-medium text-slate-700">
        <span className="text-base text-amber-500">⚠️</span>
        <strong className="text-red-500 text-sm">2</strong> Sắp hết gói
      </div>
      
      <div className="ml-auto flex gap-2">
        <button className="px-4 py-2 bg-white border border-slate-200 rounded-xl text-[13px] text-slate-700 font-medium hover:bg-slate-50 transition-colors">
          📥 Xuất Excel
        </button>
        <button className="px-5 py-2 bg-gradient-to-br from-blue-600 to-blue-700 text-white border-none rounded-xl text-[13px] font-semibold hover:shadow-md transition-shadow whitespace-nowrap">
          + Thêm Học viên
        </button>
      </div>
    </div>
  );
};
