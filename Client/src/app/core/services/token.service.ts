import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { AuthToken } from '../interfaces/api.interface';

@Injectable({ providedIn: 'root' })
export class TokenService {
    private platformId = inject(PLATFORM_ID);
    private get isBrowser(): boolean {
        return isPlatformBrowser(this.platformId);
    }

    setToken(token: AuthToken): void {
        if (!this.isBrowser) return;
        localStorage.setItem('token', JSON.stringify(token));
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