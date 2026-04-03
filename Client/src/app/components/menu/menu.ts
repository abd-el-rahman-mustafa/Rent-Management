import { CommonModule } from '@angular/common';
import { Component, output, signal } from '@angular/core';
import {  RouterLinkActive } from '@angular/router';
import { BaseComponent } from '../../core/components/base-component/base-component';
import { LangRouterLinkDirective } from '../../core/directives/lang-router-link.directive';


interface NavItem {
  label: string;
  route: string;
  icon: string;
  badge?: number;
}
@Component({
  selector: 'app-menu',
  imports: [LangRouterLinkDirective, RouterLinkActive, CommonModule],
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
      icon: 'fa-solid fa-table-cells-large',
    },
    {
      label: 'Properties',
      route: '/properties',
      icon: 'fa-solid fa-building',
    },
    {
      label: 'Tenants',
      route: '/tenants',
      icon: 'fa-solid fa-users',
      badge: 3,
    },
    {
      label: 'Contracts',
      route: '/contracts',
      icon: 'fa-solid fa-file-signature',
    },
    {
      label: 'Payments',
      route: '/payments',
      icon: 'fa-regular fa-credit-card',
    },
    {
      label: 'Reports',
      route: '/reports',
      icon: 'fa-solid fa-chart-column',
    },
    {
      label: 'Settings',
      route: '/settings',
      icon: 'fa-solid fa-gear',
    },
  ];
 
   toggle(): void {
    this.collapsed.update(v => !v);
    this.collapseChange.emit(this.collapsed());
  }
}
