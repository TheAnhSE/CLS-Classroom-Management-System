import type { PagedResponse } from '../../../shared/types/api.types';

export interface ParentDto {
  parentId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
}

export interface LearnerDto {
  learnerId: number;
  firstName: string;
  lastName: string;
  dateOfBirth?: string;
  gender?: string;
  enrollmentDate: string;
  status: string;
  notes?: string;
  parentId?: number;
  parent?: ParentDto;
}

// Map PageResponse generic to Learners
export type LearnerPagedResponse = PagedResponse<LearnerDto>;
