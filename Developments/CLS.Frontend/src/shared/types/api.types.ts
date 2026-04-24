export interface ApiResponse<T> {
  code: number;
  message: string;
  data: T;
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
