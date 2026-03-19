import { Injectable, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ApiResponse } from '../interfaces/api.interface';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
    private router = inject(Router);
    private toastr = inject(ToastrService);

    handle(apiError: HttpErrorResponse): ApiResponse<any> {
        const normalizedError = this.normalize(apiError);

        this.action(normalizedError);
        this.log(normalizedError);

        return normalizedError; // return so components can use it too
    }

    // Step A: normalize whatever the backend sends into YOUR shape
    private normalize(error: HttpErrorResponse): ApiResponse<any> {
        if (error.status === 0) {
            // Status 0 = no internet / CORS / server completely down
            return { statusCode: 0, data: '', title: 'No Internet Connection', detail: 'No internet connection. Please try again.' };
        }

        return {
            statusCode: error.status,
            data: 'null',
            detail: error.error?.detail ?? error.error?.message ?? error.message ?? 'Unexpected error',
            errors: error.error?.errors ?? null,
            title: error.error?.title ?? 'Error',
        }
    }
    // Step B:take action based on status
    private action(error: ApiResponse<any>): void {
        switch (error.statusCode) {
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
            case 400:
                this.normalizeValidationErrors(error);
                break;

            case 500:
                this.toastr.error(error.detail, error.title);
                break;
        }
        // 400, 422, 500 — stay on the same page, let the component show the error
    }

    // Step C: log to external monitoring (Sentry, Datadog, etc.)
    private log(error: ApiResponse<any>): void {
        if (error.statusCode >= 500) {
            console.error('[SERVER ERROR]', error);
        } else {
            console.warn('[CLIENT ERROR]', error);
        }
    }

    private normalizeValidationErrors(error: ApiResponse<any>): void {
        if (error.errors) {
            const messages = Object.values(error.errors).flat().join(' ');
            this.toastr.error(messages, error.title);
        } else {
            this.toastr.error(error.detail, error.title);
        }
    }
}