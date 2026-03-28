export interface ApiResponse<T> {
    data: T;
    isSuccess?: boolean;
    title?: string;
    detail?: string;
    statusCode: number;
    errors?: Record<string, string[]>; // validation errors from backend
}

export interface AuthToken {
    accessToken: string;
    accessTokenExpires: string; // ISO string
}