// add lang and platform source to all requests

import { HttpInterceptorFn } from '@angular/common/http';
import { LanguageService } from '../services/language.service';
import { inject } from '@angular/core';




export const headerInterceptor: HttpInterceptorFn = (req, next) => {

    let languageService = inject(LanguageService);
    req = req.clone({
        setHeaders: {
            'Accept-Language': languageService.lang(),
            'X-Platform': 'web' // or dynamically determine this
        }
    });
    return next(req);
}
