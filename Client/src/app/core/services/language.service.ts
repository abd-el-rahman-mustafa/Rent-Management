import { Injectable, signal, inject } from '@angular/core';
import { Router } from '@angular/router';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Lang } from '../interfaces/language.interface';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  lang = signal<Lang>(
    (isPlatformBrowser(this.platformId)
      ? localStorage.getItem('lang') as Lang
      : null) ?? 'ar'
  );

  /** Syncs lang signal + DOM only — no navigation. Used by the lang guard. */
  syncLang(lang: Lang) {
    this.lang.set(lang);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('lang', lang);
      document.dir = lang === 'ar' ? 'rtl' : 'ltr';
    }
  }

  /** Full language switch: syncs state then navigates to the new URL. */
  setLang(lang: Lang) {
    this.syncLang(lang);
    const currentUrl = this.router.url;
    const newUrl = currentUrl.replace(/^\/(ar|en)/, `/${lang}`);
    this.router.navigateByUrl(newUrl);
  }
}