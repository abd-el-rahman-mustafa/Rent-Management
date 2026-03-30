import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LanguageService } from '../services/language.service';
import { defaultLang } from '../interfaces/language.interface';

export const redirectToLangGuard: CanActivateFn = () => {
  const router = inject(Router);
  const languageService = inject(LanguageService);

  return router.createUrlTree([languageService.lang() ?? defaultLang]);
};