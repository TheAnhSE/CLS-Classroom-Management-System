export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
  errorCode?: string;
}

export interface PagedResponse<T> {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalRecords: number;
  data: T[];
}

export interface BaseEntity {
  id: number;
  createdTime?: string;
  createdBy?: number;
  updatedTime?: string;
  updatedBy?: number;
}
