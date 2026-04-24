import React from 'react';
import { useDashboard } from '../hooks/useDashboard';

export const DashboardPage: React.FC = () => {
  const { stats, isLoading, error } = useDashboard();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-red-50 text-red-700 p-4 rounded-lg border border-red-200">
        {error}
      </div>
    );
  }

  const currentDate = new Date().toLocaleDateString('vi-VN', {
    weekday: 'long',
    year: 'numeric',
    month: 'numeric',
    day: 'numeric',
  });

  return (
    <div className="max-w-[1200px] mx-auto">
      {/* Greeting */}
      <div className="mb-6">
        <h1 className="text-[21px] font-bold text-slate-900 mb-1">Chào buổi chiều, Giáo Vụ! 👋</h1>
        <p className="text-[13px] text-slate-500">
          Hôm nay: {currentDate} — {stats?.sessionsToday || 0} buổi học đang diễn ra
        </p>
      </div>

      {/* KPI Grid */}
      <div className="grid grid-cols-4 gap-4 mb-6">
        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="text-[22px] mb-2.5">👥</div>
          <div className="text-[28px] font-bold text-slate-900 leading-tight">
            {stats?.totalLearners || 0}
          </div>
          <div className="text-xs text-slate-500 mt-1">Học viên hệ thống</div>
          <div className="text-[11px] font-medium text-emerald-500 mt-1.5">↑ Dữ liệu thực</div>
        </div>

        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="text-[22px] mb-2.5">📦</div>
          <div className="text-[28px] font-bold text-slate-900 leading-tight">
            {stats?.activePackages || 0}
          </div>
          <div className="text-xs text-slate-500 mt-1">Gói học đang Active</div>
          <div className="text-[11px] font-medium text-slate-500 mt-1.5">Dữ liệu thực</div>
        </div>

        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="text-[22px] mb-2.5">📅</div>
          <div className="text-[28px] font-bold text-slate-900 leading-tight">
            {stats?.sessionsToday || 0}
          </div>
          <div className="text-xs text-slate-500 mt-1">Buổi học hôm nay</div>
          <div className="text-[11px] font-medium text-blue-500 mt-1.5">Trạng thái Scheduled</div>
        </div>

        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="text-[22px] mb-2.5">💰</div>
          <div className="text-[28px] font-bold text-slate-900 leading-tight">
            {stats?.totalRevenue ? new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(stats.totalRevenue) : '0 ₫'}
          </div>
          <div className="text-xs text-slate-500 mt-1">Tổng doanh thu gói học</div>
          <div className="text-[11px] font-medium text-emerald-500 mt-1.5">↑ Dữ liệu thực</div>
        </div>
      </div>

      {/* Alert Banner */}
      <div className="bg-gradient-to-br from-amber-100 to-amber-200 border border-amber-400 rounded-xl p-3.5 flex items-center gap-3 mb-6">
        <span className="text-lg">⚠️</span>
        <span className="text-[13px] text-amber-900 font-medium">
          <strong>Cảnh báo:</strong> 12 học viên sẽ hết Learning Package trong 14 ngày — liên hệ gia hạn ngay!
        </span>
        <div className="ml-auto bg-amber-500 text-white text-xs font-bold px-2.5 py-1 rounded-full">
          12 HV
        </div>
      </div>

      {/* Bottom Grid */}
      <div className="grid grid-cols-[1.4fr_1fr] gap-5">
        {/* Schedule Card */}
        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-sm font-bold text-slate-900">📅 Lịch học hôm nay</h2>
            <button className="text-xs text-blue-600 font-medium hover:underline">
              Xem toàn bộ →
            </button>
          </div>
          
          <div className="space-y-0">
            {/* Session Items - Static for now */}
            {[
              { time: '08:00–09:30', name: 'Tiếng Anh – Lớp A1', meta: 'GV: Nguyễn Văn A · Phòng 101', status: 'Đang học', statusColor: 'bg-green-100 text-green-700' },
              { time: '09:45–11:15', name: 'Kids Coding – Python Cơ bản', meta: 'GV: Trần Thị B · Phòng 102', status: 'Sắp tới', statusColor: 'bg-blue-50 text-blue-700' },
              { time: '14:00–15:30', name: 'Tiếng Anh – IELTS Prep', meta: 'GV: Lê Văn C · Phòng 103', status: 'Sắp tới', statusColor: 'bg-blue-50 text-blue-700' },
              { time: '15:45–17:15', name: 'Tiếng Anh – Giao tiếp B2', meta: 'GV: Phạm Thị D · Phòng 101', status: 'Sắp tới', statusColor: 'bg-blue-50 text-blue-700' }
            ].map((s, idx) => (
              <div key={idx} className="flex items-center gap-3 py-2.5 border-b border-slate-50 last:border-0">
                <div className="bg-blue-50 text-blue-700 text-[11px] font-semibold px-2 py-1 rounded-md whitespace-nowrap">
                  {s.time}
                </div>
                <div className="flex-1">
                  <div className="text-[13px] font-semibold text-slate-900">{s.name}</div>
                  <div className="text-[11px] text-slate-400 mt-0.5">{s.meta}</div>
                </div>
                <div className={`text-[11px] font-semibold px-2 py-1 rounded-md ${s.statusColor}`}>
                  {s.status}
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Alerts Card */}
        <div className="bg-white rounded-xl p-5 shadow-sm border border-slate-100">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-sm font-bold text-slate-900">🔔 Cảnh báo hệ thống</h2>
            <button className="text-xs text-blue-600 font-medium hover:underline">
              Xem tất cả
            </button>
          </div>

          <div className="space-y-0">
            {[
              { color: 'bg-red-500', msg: <><strong className="text-slate-700">SLA vi phạm:</strong> GV Nguyễn Văn A chưa nộp feedback lớp A1.</>, time: '2 giờ trước' },
              { color: 'bg-amber-500', msg: <><strong className="text-slate-700">Gói sắp hết:</strong> HV Minh Anh còn 2 buổi – Python.</>, time: '5 giờ trước' },
              { color: 'bg-blue-500', msg: <><strong className="text-slate-700">Điểm danh chưa ghi:</strong> Lớp IELTS 22/04.</>, time: '1 ngày trước' },
              { color: 'bg-amber-500', msg: <><strong className="text-slate-700">Gói sắp hết:</strong> HV Tuấn Kiệt còn 3 buổi – B2.</>, time: '1 ngày trước' }
            ].map((a, idx) => (
              <div key={idx} className="flex gap-2.5 py-2.5 border-b border-slate-50 last:border-0">
                <div className={`w-2 h-2 rounded-full mt-1.5 shrink-0 ${a.color}`}></div>
                <div>
                  <div className="text-xs text-slate-600 leading-relaxed">{a.msg}</div>
                  <div className="text-[11px] text-slate-400 mt-0.5">{a.time}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};
