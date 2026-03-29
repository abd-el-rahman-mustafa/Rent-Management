import { Injectable, signal } from '@angular/core';
import { Lang } from '../interfaces/language.interface';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  lang = signal<Lang>(
    (localStorage.getItem('lang') as Lang) ?? 'en'
  );

  setLang(lang: Lang) {
    this.lang.set(lang);
    localStorage.setItem('lang', lang);
    document.dir = lang === 'ar' ? 'rtl' : 'ltr';
  }
}