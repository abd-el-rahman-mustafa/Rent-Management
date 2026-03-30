import {
  HttpErrorResponse,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, finalize, retry, throwError, timer } from 'rxjs';
import { ErrorHandlerService } from '../services/error-handler.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorHandler = inject(ErrorHandlerService);

  return next(req).pipe(

    // ── RETRY LOGIC ──────────────────────────────────────────
    // Only retry on network errors (status 0) or server overload (503/504)
    // Do NOT retry on 401, 403, 404 — those won't magically fix themselves 
    retry({
      count: 2,
      delay: (error: HttpErrorResponse, attempt) => {
        const retryable = [0, 503, 504];
        if (!retryable.includes(error.status)) {
          return throwError(() => error); // give up immediately
        }
        // Exponential backoff: 1s, 2s, 4s...
        return timer(1000 * Math.pow(2, attempt - 1));
      }
    }),

    // ── ERROR CATCHING ────────────────────────────────────────
    catchError((error: HttpErrorResponse) => {
      const apiError = errorHandler.handle(error); // normalize + route + log
      return throwError(() => apiError); // re-throw so component's error callback fires
    }),

    // ── CLEANUP ───────────────────────────────────────────────
    // finalize ALWAYS runs — whether success, error, or unsubscribe
    finalize(() => { })
  );
};