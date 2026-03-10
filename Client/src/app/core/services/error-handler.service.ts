import { Injectable, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ApiResponse } from '../interfaces/api.interface';

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
    private router = inject(Router);

    handle(apiError: HttpErrorResponse): ApiResponse<any> {
        const normalizedError = this.normalize(apiError);

        this.route(normalizedError);
        this.log(normalizedError);

        return normalizedError; // return so components can use it too
    }

    // Step A: normalize whatever the backend sends into YOUR shape
    private normalize(error: HttpErrorResponse): ApiResponse<any> {
        if (error.status === 0) {
            // Status 0 = no internet / CORS / server completely down
            return { status: 0, title: 'No Internet Connection', details: 'No internet connection. Please try again.' };
        }

        return {
            status: error.status,
            details: error.error?.message ?? error.message ?? 'Unexpected error',
            errors: error.error?.errors ?? null,
            title: error.error?.title ?? 'Error',
        }
    }
    // Step B: navigate based on status
    private route(error: ApiResponse<any>): void {
        switch (error.status) {
            case 401:
                localStorage.removeItem('token');
                this.router.navigate(['/login']);
                break;
            case 403:
                this.router.navigate(['/forbidden']);
                break;
            case 404:
                this.router.navigate(['/not-found']);
                break;
        }
        // 400, 422, 500 — stay on the same page, let the component show the error
    }

    // Step C: log to external monitoring (Sentry, Datadog, etc.)
    private log(error: ApiResponse<any>): void {
        if (error.status >= 500) {
            console.error('[SERVER ERROR]', error);
        } else {
            console.warn('[CLIENT ERROR]', error);
        }
    }
}