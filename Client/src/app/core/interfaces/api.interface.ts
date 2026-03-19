export interface ApiResponse<T> {
    data: T;
    isSuccess?: boolean;
    title?: string;
    detail?: string;
    statusCode: number;
    errors?: Record<string, string[]>; // validation errors from backend
}

