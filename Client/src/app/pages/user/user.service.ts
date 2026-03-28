import { inject, Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimpleUserInfo } from '../../shared/interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {
    url = environment.API_URL + 'user';
  http = inject(HttpClient);


  // get all users
  getAllUsers():Observable<SimpleUserInfo[]> {
    return this.http.get<SimpleUserInfo[]>(`${this.url}/all`);
  }
}
