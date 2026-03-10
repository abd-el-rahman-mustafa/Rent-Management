export interface ApiResponse<T> {
    data?: T;
    isSuccess?: boolean;
    title?: string;
    details?: string;
    status: number;
    errors?: Record<string, string[]>; // validation errors from backend
}
