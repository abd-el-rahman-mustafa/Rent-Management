import { CommonModule } from '@angular/common';
import { Component, output, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BaseComponent } from '../../core/components/base-component/base-component';


interface NavItem {
  label: string;
  route: string;
  icon: string;
  badge?: number;
}
@Component({
  selector: 'app-menu',
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})

export class MenuComponent extends BaseComponent {
collapsed = signal(false);
  collapseChange = output<boolean>();
 
  navItems: NavItem[] = [
    {
      label: 'Dashboard',
      route: '/dashboard',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <rect x="3" y="3" width="7" height="7" rx="1"/>
               <rect x="14" y="3" width="7" height="7" rx="1"/>
               <rect x="3" y="14" width="7" height="7" rx="1"/>
               <rect x="14" y="14" width="7" height="7" rx="1"/>
             </svg>`,
    },
    {
      label: 'Properties',
      route: '/properties',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <path d="M3 9.5L12 3l9 6.5V21H3V9.5z"/>
               <rect x="9" y="14" width="6" height="7"/>
             </svg>`,
    },
    {
      label: 'Tenants',
      route: '/tenants',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <circle cx="12" cy="7" r="4"/>
               <path d="M4 21c0-4 3.6-7 8-7s8 3 8 7"/>
             </svg>`,
      badge: 3,
    },
    {
      label: 'Contracts',
      route: '/contracts',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/>
               <polyline points="14,2 14,8 20,8"/>
               <line x1="8" y1="13" x2="16" y2="13"/>
               <line x1="8" y1="17" x2="12" y2="17"/>
             </svg>`,
    },
    {
      label: 'Payments',
      route: '/payments',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <rect x="2" y="5" width="20" height="14" rx="2"/>
               <line x1="2" y1="10" x2="22" y2="10"/>
             </svg>`,
    },
    {
      label: 'Reports',
      route: '/reports',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <line x1="18" y1="20" x2="18" y2="10"/>
               <line x1="12" y1="20" x2="12" y2="4"/>
               <line x1="6"  y1="20" x2="6"  y2="14"/>
             </svg>`,
    },
    {
      label: 'Settings',
      route: '/settings',
      icon: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
               <circle cx="12" cy="12" r="3"/>
               <path d="M19.4 15a1.65 1.65 0 00.33 1.82l.06.06a2 2 0 010 2.83 2 2 0 01-2.83 0l-.06-.06a1.65 1.65 0 00-1.82-.33 1.65 1.65 0 00-1 1.51V21a2 2 0 01-4 0v-.09A1.65 1.65 0 009 19.4a1.65 1.65 0 00-1.82.33l-.06.06a2 2 0 01-2.83-2.83l.06-.06A1.65 1.65 0 004.68 15a1.65 1.65 0 00-1.51-1H3a2 2 0 010-4h.09A1.65 1.65 0 004.6 9a1.65 1.65 0 00-.33-1.82l-.06-.06a2 2 0 012.83-2.83l.06.06A1.65 1.65 0 009 4.68a1.65 1.65 0 001-1.51V3a2 2 0 014 0v.09a1.65 1.65 0 001 1.51 1.65 1.65 0 001.82-.33l.06-.06a2 2 0 012.83 2.83l-.06.06A1.65 1.65 0 0019.4 9a1.65 1.65 0 001.51 1H21a2 2 0 010 4h-.09a1.65 1.65 0 00-1.51 1z"/>
             </svg>`,
    },
  ];
 
  toggle(): void {
    this.collapsed.update(v => !v);
    this.collapseChange.emit(this.collapsed());
  }
}
