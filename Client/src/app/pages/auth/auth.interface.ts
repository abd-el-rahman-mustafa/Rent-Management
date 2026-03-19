export interface RegisterDto {
    firstName: string;
    lastName: string;
    email: string;
    emailOtpCode: string;
    password: string;
    confirmPassword: string;
}

export interface LoginDto {
    email: string;
    password: string;
}

export interface LoginOtpDto {
    email: string;
    otp: string;
}

export interface AuthToken {
    accessToken: string;
    accessTokenExpires: string; // ISO string
}