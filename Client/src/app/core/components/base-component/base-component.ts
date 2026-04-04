import { Component, inject, OnDestroy, PLATFORM_ID } from '@angular/core';
import { LanguageService } from '../../services/language.service';
import { UserTokenData } from '../../../pages/auth/auth.interface';
import { TokenService } from '../../services/token.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-base-component',
  imports: [],
  templateUrl: './base-component.html',
  styleUrl: './base-component.css',
})
export class BaseComponent {

  private langService = inject(LanguageService);
  tokenService = inject(TokenService);

  user: UserTokenData | null = null;
  lang = this.langService.lang;
  toggleLang = () => this.langService.toggleLang();

    // isBrowser is a getter that checks if the code is running in the browser, which is necessary for accessing localStorage
  private platformId = inject(PLATFORM_ID);
   get isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

   constructor() {
    const token = this.tokenService.getAccessToken();

    // save user data in a property for use in the app, this is set when the token is handled in the TokenService
    if (token && this.isBrowser) {
      const userData = localStorage.getItem('user');
      if (userData) {
        this.user = JSON.parse(userData);
      }
    }
  }






}
