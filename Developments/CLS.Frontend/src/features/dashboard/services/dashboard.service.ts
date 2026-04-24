import apiClient from '../../../shared/services/api.client';
import type { ApiResponse } from '../../../shared/types/api.types';
import type { DashboardStatsDto } from '../types/dashboard.types';

export const dashboardService = {
  getSummaryStats: async (): Promise<ApiResponse<DashboardStatsDto>> => {
    return await apiClient.get<any, ApiResponse<DashboardStatsDto>>('/dashboard/summary');
  },
};
