import React from 'react';
import type { LearnerDto } from '../types/learner.types';

interface TableProps {
  learners: LearnerDto[];
  isLoading: boolean;
  pageNumber: number;
  totalPages: number;
  totalRecords: number;
  onPageChange: (page: number) => void;
}

export const LearnerTable: React.FC<TableProps> = ({ 
  learners, isLoading, pageNumber, totalPages, totalRecords, onPageChange 
}) => {
  // Helper to get initials for avatar
  const getInitials = (firstName: string, lastName: string) => {
    return `${firstName?.charAt(0) || ''}${lastName?.charAt(0) || ''}`.toUpperCase();
  };

  // Helper to map status to styling
  const getStatusStyle = (status: string) => {
    switch(status) {
      case 'Active': return 'bg-green-100 text-green-700';
      case 'Inactive': return 'bg-slate-100 text-slate-500';
      case 'Warning': return 'bg-amber-100 text-amber-700';
      default: return 'bg-blue-50 text-blue-700';
    }
  };

  const getStatusText = (status: string) => {
    switch(status) {
      case 'Active': return '✅ Hoạt động';
      case 'Inactive': return '⏸ Tạm dừng';
      case 'Warning': return '⚠️ Sắp hết';
      default: return status;
    }
  };

  return (
    <div className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
      <div className="overflow-x-auto">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 border-b border-slate-200">
              <th className="py-3 px-4 w-12 text-center"><input type="checkbox" className="rounded border-slate-300" /></th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Học viên</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Phụ huynh / Email</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Môn học</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Gói học phí</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Buổi còn lại</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider">Trạng thái</th>
              <th className="py-3 px-4 text-xs font-semibold text-slate-500 uppercase tracking-wider text-center">Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={8} className="py-12 text-center text-slate-400">
                  <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto mb-2"></div>
                  Đang tải danh sách...
                </td>
              </tr>
            ) : learners.length === 0 ? (
              <tr>
                <td colSpan={8} className="py-8 text-center text-slate-500">
                  Không tìm thấy học viên nào.
                </td>
              </tr>
            ) : (
              learners.map((l, idx) => {
                // Dummy values for missing fields
                const courseName = idx % 2 === 0 ? 'Tiếng Anh' : 'Kids Coding';
                const pkgName = idx % 2 === 0 ? 'English Pro – 40 buổi' : 'Coding Starter – 20 buổi';
                const remainSessions = idx % 3 === 0 ? 2 : 12;
                const progressWidth = idx % 3 === 0 ? '10%' : '60%';
                const progressColor = idx % 3 === 0 ? 'from-red-500 to-red-400' : 'from-blue-600 to-blue-400';
                
                // Mapped Status logic (Since backend returns string, we might need to map it)
                // For demonstration, map the first item to Warning if not active
                const displayStatus = remainSessions <= 3 ? 'Warning' : (l.status || 'Active');

                return (
                  <tr key={l.learnerId} className="border-b border-slate-50 hover:bg-slate-50/50 transition-colors">
                    <td className="py-3.5 px-4 text-center">
                      <input type="checkbox" className="rounded border-slate-300" />
                    </td>
                    <td className="py-3.5 px-4">
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-lg bg-gradient-to-br from-indigo-500 to-purple-500 flex items-center justify-center text-white text-[11px] font-bold shrink-0">
                          {getInitials(l.firstName, l.lastName)}
                        </div>
                        <div>
                          <div className="text-[13px] font-semibold text-slate-900">{l.lastName} {l.firstName}</div>
                          <div className="text-[11px] text-slate-400 mt-0.5">#LRN-{l.learnerId.toString().padStart(3, '0')}</div>
                        </div>
                      </div>
                    </td>
                    <td className="py-3.5 px-4">
                      <div className="text-[13px] font-medium text-slate-700">{l.parent ? `${l.parent.lastName} ${l.parent.firstName}` : '—'}</div>
                      <div className="text-[11px] text-slate-400">{l.parent?.email || '—'}</div>
                    </td>
                    <td className="py-3.5 px-4 text-[13px] text-slate-700">{courseName}</td>
                    <td className="py-3.5 px-4 text-[13px] text-slate-700">{pkgName}</td>
                    <td className="py-3.5 px-4">
                      <div className="flex items-center gap-2">
                        <div className="w-20 h-1.5 bg-slate-200 rounded-full overflow-hidden">
                          <div className={`h-full bg-gradient-to-r ${progressColor} rounded-full`} style={{ width: progressWidth }}></div>
                        </div>
                        <span className={`text-xs font-semibold ${remainSessions <= 3 ? 'text-red-500' : 'text-slate-600'}`}>
                          {remainSessions} buổi
                        </span>
                      </div>
                    </td>
                    <td className="py-3.5 px-4">
                      <span className={`text-[11px] font-semibold px-2.5 py-1 rounded-full ${getStatusStyle(displayStatus)}`}>
                        {getStatusText(displayStatus)}
                      </span>
                    </td>
                    <td className="py-3.5 px-4 text-center">
                      <button className="px-3 py-1.5 border-1.5 border-slate-200 rounded-lg text-xs font-medium text-slate-600 hover:bg-slate-100 hover:text-slate-900 transition-colors">
                        Xem →
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      <div className="px-5 py-3.5 border-t border-slate-200 flex items-center justify-between">
        <div className="text-[13px] text-slate-500">
          Hiển thị {(pageNumber - 1) * 6 + 1}–{Math.min(pageNumber * 6, totalRecords)} / {totalRecords} học viên
        </div>
        <div className="flex gap-1.5">
          <button 
            disabled={pageNumber === 1}
            onClick={() => onPageChange(pageNumber - 1)}
            className="w-8 h-8 flex items-center justify-center rounded-lg border-1.5 border-slate-200 text-slate-600 hover:bg-slate-50 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            ‹
          </button>
          
          {/* Simple logic for pages */}
          {Array.from({ length: Math.min(totalPages, 5) }).map((_, i) => {
            const pageNum = i + 1;
            const isActive = pageNum === pageNumber;
            return (
              <button 
                key={pageNum}
                onClick={() => onPageChange(pageNum)}
                className={`w-8 h-8 flex items-center justify-center rounded-lg border-1.5 text-[13px] transition-colors ${
                  isActive 
                    ? 'bg-blue-600 border-blue-600 text-white font-semibold shadow-sm' 
                    : 'border-slate-200 text-slate-600 hover:bg-slate-50'
                }`}
              >
                {pageNum}
              </button>
            )
          })}
          
          <button 
            disabled={pageNumber >= totalPages}
            onClick={() => onPageChange(pageNumber + 1)}
            className="w-8 h-8 flex items-center justify-center rounded-lg border-1.5 border-slate-200 text-slate-600 hover:bg-slate-50 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            ›
          </button>
        </div>
      </div>
    </div>
  );
};
