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

  url = environment.API_URL + 'auth';

  constructor(private http: HttpClient) {
  }

  sendOtp(email: string) {
    return this.http.post(`${this.url}/send-email-otp`, { email });
  }

  register(registerDtor: RegisterDto) {
    return this.http.post(`${this.url}/register`, registerDtor);
  }

  loginRequest(loginDto: LoginDto): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.url}/login-request`, loginDto);
  }
  emailOtpLogin(otpDto: LoginOtpDto): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.url}/email-otp-login`, otpDto);
  }


}
