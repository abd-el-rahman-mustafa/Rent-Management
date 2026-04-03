import { Injectable, signal, inject } from '@angular/core';
import { Router } from '@angular/router';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, DOCUMENT } from '@angular/common';
import { defaultLang, Lang } from '../interfaces/language.interface';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);
  private document = inject(DOCUMENT);

  // Default language is Arabic. This can be changed to English if needed.

  /**
   *
   */
  constructor() {
    this.syncLang(defaultLang);
  }

  lang = signal<Lang>(
    (isPlatformBrowser(this.platformId) ? (localStorage.getItem('lang') as Lang) : null) ??
      defaultLang,
  );

  /** Syncs lang signal + DOM only — no navigation. Used by the lang guard. */
  syncLang(lang: Lang) {
    this.lang.set(lang);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('lang', lang);
      const dir = lang === 'en' ? 'ltr' : 'rtl';
      this.document.documentElement.setAttribute('dir', dir);
      this.document.documentElement.setAttribute('lang', lang);
    }
  }

  /** Full language switch: syncs state then navigates to the new URL. */
  setLang(lang: Lang) {
    this.syncLang(lang);
    const currentUrl = this.router.url;
    const newUrl = currentUrl.replace(/^\/(ar|en)/, `/${lang}`);
    return this.router.navigateByUrl(newUrl);
  }

  toggleLang() {
    if (isPlatformBrowser(this.platformId)) {
      const newLang = this.lang() === 'en' ? 'ar' : 'en';
      this.setLang(newLang).then(() => {
        // reload the page to apply the new language settings cleanly
        location.reload();
      });
    }
  }
}
