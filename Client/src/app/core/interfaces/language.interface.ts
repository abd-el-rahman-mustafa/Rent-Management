export const SUPPORTED_LANGS = ['en', 'ar'] as const;
export type Lang = typeof SUPPORTED_LANGS[number]; // 'en' | 'ar'

// Default language 
// Centralized here to avoid hardcoding 'en' or 'ar' in multiple places, which can lead to bugs if we forget to change it somewhere.   
export const defaultLang: Lang = 'en';