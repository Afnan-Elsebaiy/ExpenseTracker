export interface ApiResponse<T> {
    statusCode: number;
    responseType: number;
    message: string;
    data: T | null;
    errors: string[];
  }