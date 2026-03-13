import { Register } from './register/register';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { LoginDto, LoginOtpDto, RegisterDto } from './auth.interface';

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

  login(loginDto: LoginDto) {
    return this.http.post(`${environment.API_URL}auth/login`, loginDto);
  }

  verifyLoginOtp(otpDto: LoginOtpDto) {
    return this.http.post(`${environment.API_URL}auth/verify-login-otp`, otpDto);
  }


}
