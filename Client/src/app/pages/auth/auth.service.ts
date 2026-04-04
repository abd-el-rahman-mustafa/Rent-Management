import { Register } from './register/register';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { LoginDto, LoginOtpDto, RegisterDto } from './auth.interface';
import { ApiResponse, AuthToken } from '../../core/interfaces/api.interface';
import { Observable } from 'rxjs';
import { LanguageService } from '../../core/services/language.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  url = environment.API_URL + 'auth';
  http = inject(HttpClient);
  languageService = inject(LanguageService);

  sendOtp(email: string) : Observable<ApiResponse<boolean>>{
    return this.http.post<ApiResponse<boolean>>(`${this.url}/send-email-otp`, { email });
  }

  register(registerDtor: RegisterDto) {
    return this.http.post(`${this.url}/register`, registerDtor);
  }

  loginRequest(loginDto: LoginDto): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.url}/login-request`, loginDto);
  }
  emailOtpLogin(otpDto: LoginOtpDto): Observable<ApiResponse<AuthToken>> {
    return this.http.post<ApiResponse<AuthToken>>(`${this.url}/email-otp-login`, otpDto);
  }

    getDecodedAccessToken(accessToken: string) {
      // atob is a built-in function that decodes a base64-encoded string
    return JSON.parse(atob(accessToken.split('.')[1]));
  }

  logout() {
    // Just remove the token from localStorage, the app will react to this change and update the UI accordingly (e.g., show login/register buttons instead of user info)
    localStorage.removeItem('token');
    localStorage.removeItem('user');

    //TODO: route to the home page after logout
    // window.location.href = '/';
    // router to login page after logout
    window.location.href = `${this.languageService.lang()}/login`;
  }


}
