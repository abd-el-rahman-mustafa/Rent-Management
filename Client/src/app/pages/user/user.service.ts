import { inject, Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimpleUserInfo, UpdateUserDto, User } from './user.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  url = environment.API_URL + 'user';
  http = inject(HttpClient);

  getAllUsers(): Observable<SimpleUserInfo[]> {
    return this.http.get<SimpleUserInfo[]>(`${this.url}/all`);
  }

  getUserById(id: string | number): Observable<User> {
    return this.http.get<User>(`${this.url}/${id}`);
  }

  updateUser(id: string | number, payload: UpdateUserDto): Observable<User> {
    return this.http.put<User>(`${this.url}/${id}`, payload);
  }
}