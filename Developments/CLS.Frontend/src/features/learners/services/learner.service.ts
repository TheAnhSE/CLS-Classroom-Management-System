import apiClient from '../../../shared/services/api.client';
import type { LearnerPagedResponse } from '../types/learner.types';

export const learnerService = {
  getAllLearners: async (pageNumber: number = 1, pageSize: number = 10): Promise<LearnerPagedResponse> => {
    // Note: LearnerPagedResponse is PagedResponse, which already unwraps ApiResponse.data in interceptor.
    // However, the interceptor unwraps response.data. The backend LearnerController returns Ok(pagedResponse).
    // Let's ensure the type is correct. The interceptor currently returns response.data directly.
    return await apiClient.get<any, LearnerPagedResponse>(`/learners?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  },
};
