import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
    private requestCount = 0;

    isLoading = signal(false);

    show() {
        setTimeout(() => {
            if (this.requestCount > 0) {
                this.isLoading.set(true);
            }
        }, 150);
    }

    hide() {
        this.requestCount--;

        if (this.requestCount <= 0) {
            this.isLoading.set(false);
            this.requestCount = 0;
        }
    }
}