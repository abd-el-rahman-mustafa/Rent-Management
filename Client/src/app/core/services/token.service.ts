
// set and get token from local storage

import { Injectable } from '@angular/core';
import { AuthToken } from '../interfaces/api.interface';

@Injectable({
    providedIn: 'root'
})
export class TokenService {
    private TOKEN_KEY: AuthToken = {
        accessToken: '',
        accessTokenExpires: ''
    };

    setToken(token: AuthToken) {
        localStorage.setItem('token', JSON.stringify(token));
    }

    getToken(): AuthToken | null {
        const token = localStorage.getItem('token');
        return token ? JSON.parse(token) : null;
    }

    getAccessToken(): string | null {
        const token = this.getToken();
        return token ? token.accessToken : null;
    }

    getTokenExpiration(): Date | null {
        const token = this.getToken();
        return token ? new Date(token.accessTokenExpires) : null;
    }

    removeToken() {
        localStorage.removeItem('token');
    }

}