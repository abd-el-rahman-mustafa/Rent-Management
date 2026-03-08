import { Register } from './register/register';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { RegisterDto } from './auth.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {


  constructor(private http: HttpClient) {
  }

  register(registerDtor: RegisterDto) {
    return this.http.post(`${environment.API_URL}auth/register`, registerDtor);
  }


}
