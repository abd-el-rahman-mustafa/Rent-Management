import { inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { Lang } from '../interfaces/language.interface';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class LanguageService {

  platformId = inject(PLATFORM_ID);

  lang = signal<Lang>(
    isPlatformBrowser(this.platformId)
      ? (localStorage.getItem('lang') as Lang) ?? 'ar'
      : 'ar'
  );

  setLang(lang: Lang) {
    this.lang.set(lang);
    localStorage.setItem('lang', lang);
    if (isPlatformBrowser(this.platformId)) {
      document.dir = lang === 'ar' ? 'rtl' : 'ltr';
    }
  }
}