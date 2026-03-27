import { inject, Injectable } from '@angular/core';
import { environment } from '../../../env/env.dev';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class RoleService {

    url = environment.API_URL + 'role';
  http = inject(HttpClient);
  

  // get all roles
  getAllRoles() {
    return this.http.get(`${this.url}/get-all-roles`);
  }
}
