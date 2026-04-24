import React from 'react';
import { LearnerStats } from '../components/LearnerStats';
import { LearnerFilterBar } from '../components/LearnerFilterBar';
import { LearnerTable } from '../components/LearnerTable';
import { useLearners } from '../hooks/useLearners';

export const LearnerListPage: React.FC = () => {
  const { 
    learners, 
    isLoading, 
    error, 
    pageNumber, 
    setPageNumber, 
    totalPages, 
    totalRecords 
  } = useLearners(1, 6);

  return (
    <div className="max-w-[1200px] mx-auto">
      {/* Breadcrumb - similar to mockup */}
      <div className="flex items-center gap-2 mb-6">
        <span className="text-[13px] text-slate-500">Dashboard</span>
        <span className="text-slate-400">›</span>
        <span className="text-[13px] font-semibold text-slate-900">Danh sách Học viên</span>
      </div>

      {error && (
        <div className="bg-red-50 text-red-700 p-4 rounded-lg border border-red-200 mb-6">
          {error}
        </div>
      )}

      {/* Stats Bar */}
      <LearnerStats total={totalRecords} />

      {/* Filter Bar */}
      <LearnerFilterBar />

      {/* Main Table */}
      <LearnerTable 
        learners={learners}
        isLoading={isLoading}
        pageNumber={pageNumber}
        totalPages={totalPages}
        totalRecords={totalRecords}
        onPageChange={setPageNumber}
      />
    </div>
  );
};
