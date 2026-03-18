import { Register } from './register/register';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { LoginDto, LoginOtpDto, LoginResponse, RegisterDto } from './auth.interface';
import { ApiResponse } from '../../core/interfaces/api.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {


  constructor(private http: HttpClient) {
  }

  sendOtp(email: string) {
    return this.http.post(`${environment.API_URL}auth/send-email-otp`, { email });
  }

  register(registerDtor: RegisterDto) {
    return this.http.post(`${environment.API_URL}auth/register`, registerDtor);
  }

  loginRequest(loginDto: LoginDto): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${environment.API_URL}auth/login-request`, loginDto);
  }
  emailOtpLogin(otpDto: LoginOtpDto) : Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${environment.API_URL}auth/email-otp-login`, otpDto);
  }


}
