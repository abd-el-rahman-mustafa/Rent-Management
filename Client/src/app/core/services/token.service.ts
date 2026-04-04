import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { AuthToken } from '../interfaces/api.interface';
import { UserTokenData } from '../../pages/auth/auth.interface';

@Injectable({ providedIn: 'root' })
export class TokenService {
    private platformId = inject(PLATFORM_ID);
    private get isBrowser(): boolean {
        return isPlatformBrowser(this.platformId);
    }

    handleToken(token: AuthToken): void {

        // 1- Store the token in localStorage
        if (!this.isBrowser) return;
        localStorage.setItem('token', JSON.stringify(token));
        // 2- Decode the token to extract user data and store it in a property for use in the app
        try {
            let decodedToken = JSON.parse(atob(token.accessToken.split('.')[1]));
            let user: UserTokenData = {
                id: decodedToken.nameid,
                email: decodedToken.email,
                fullName: decodedToken.unique_name,
                roles: decodedToken.role,
            }
            // store the user data in localStorage as well, so it can be used across the app without needing to decode the token multiple times
            localStorage.setItem('user', JSON.stringify(user));
        } catch (error) {
            console.error('Error decoding token:', error);
            return;
        }

    }

    getToken(): AuthToken | null {
        if (!this.isBrowser) return null;
        const token = localStorage.getItem('token');
        return token ? JSON.parse(token) : null;
    }

    getAccessToken(): string | null {
        return this.getToken()?.accessToken ?? null;
    }

    getTokenExpiration(): Date | null {
        const token = this.getToken();
        return token ? new Date(token.accessTokenExpires) : null;
    }

    removeToken(): void {
        if (!this.isBrowser) return;
        localStorage.removeItem('token');
    }
}