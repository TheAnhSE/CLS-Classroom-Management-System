import { useState, useEffect } from 'react';
import { learnerService } from '../services/learner.service';
import type { LearnerDto } from '../types/learner.types';

export const useLearners = (initialPage: number = 1, pageSize: number = 6) => {
  const [learners, setLearners] = useState<LearnerDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Pagination state
  const [pageNumber, setPageNumber] = useState(initialPage);
  const [totalPages, setTotalPages] = useState(1);
  const [totalRecords, setTotalRecords] = useState(0);

  const fetchLearners = async (page: number) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await learnerService.getAllLearners(page, pageSize);
      // Depending on interceptor behavior, response might be the actual data
      if (response && response.data) {
        setLearners(response.data);
        setTotalPages(response.totalPages);
        setTotalRecords(response.totalRecords);
      }
    } catch (err: any) {
      setError(err.message || 'Lỗi kết nối máy chủ khi lấy danh sách học viên.');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchLearners(pageNumber);
  }, [pageNumber]);

  return {
    learners,
    isLoading,
    error,
    pageNumber,
    setPageNumber,
    totalPages,
    totalRecords,
    refetch: () => fetchLearners(pageNumber)
  };
};
