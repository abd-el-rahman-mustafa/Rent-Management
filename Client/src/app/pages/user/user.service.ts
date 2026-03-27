import { inject, Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
    url = environment.API_URL + 'user';
  http = inject(HttpClient);


  // get all users
  getAllUsers() {
    return this.http.get(`${this.url}/get-all-users`);
  }
}
