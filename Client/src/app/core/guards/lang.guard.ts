// guards/lang.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { LanguageService } from '../services/language.service';
import { Lang, SUPPORTED_LANGS } from '../interfaces/language.interface';


export const langGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const router = inject(Router);
  const languageService = inject(LanguageService);

  const lang = route.paramMap.get('lang') as Lang;

  if (!SUPPORTED_LANGS.includes(lang)) {
    return router.createUrlTree([languageService.lang(), '404']);
  }

  // Sync the signal + DOM with the URL lang.
  // syncLang() is used here (not setLang) to avoid triggering a router
  // navigation inside a guard, which would cause an infinite redirect loop.
  if (languageService.lang() !== lang) {
    languageService.syncLang(lang);
  }

  return true;
};