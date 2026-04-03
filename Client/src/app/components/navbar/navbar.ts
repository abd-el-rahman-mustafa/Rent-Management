import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { LanguageService } from '../../core/services/language.service'; // adjust path as needed
import { BaseComponent } from '../../core/components/base-component/base-component';
import { LanguageBtn } from "../../shared/components/language-btn/language-btn";

const ROUTE_TITLES: Record<string, string> = {
  '/dashboard':  'Dashboard',
  '/properties': 'Properties',
  '/tenants':    'Tenants',
  '/contracts':  'Contracts',
  '/payments':   'Payments',
  '/reports':    'Reports',
  '/settings':   'Settings',
};

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, LanguageBtn],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class NavbarComponent extends BaseComponent {
  private router = inject(Router);


  private currentUrl = toSignal(
    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd),
      map(e => (e as NavigationEnd).urlAfterRedirects)
    ),
    { initialValue: this.router.url }
  );

  pageTitle = computed(() => {
    const url = this.currentUrl() ?? '';
    const matched = Object.keys(ROUTE_TITLES).find(k => url.startsWith(k));
    return matched ? ROUTE_TITLES[matched] : 'rent ms';
  });

}