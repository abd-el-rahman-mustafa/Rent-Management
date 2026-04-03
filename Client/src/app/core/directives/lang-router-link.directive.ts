// directives/lang-router-link.directive.ts
import { Directive, Input, inject, forwardRef } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { LanguageService } from '../services/language.service';

@Directive({
  selector: '[routerLink]', // override the default RouterLink
  standalone: true,
  providers: [
    {
      provide: RouterLink,
      useExisting: forwardRef(() => LangRouterLinkDirective),
    },
  ],
})
export class LangRouterLinkDirective extends RouterLink {
  private languageService = inject(LanguageService);

  @Input()
  override set routerLink(commands: any[] | string) {
    const lang = this.languageService.lang();

    if (typeof commands === 'string') {
      super.routerLink = `/${lang}${commands}`;
    } else {
      super.routerLink = [`/${lang}`, ...commands.filter((s) => s !== '/')];
    }
  }
}
