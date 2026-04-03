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
      label: this.lang()== 'en'? 'Dashboard' : 'لوحة التحكم',
      route: '/dashboard',
      icon: 'fa-solid fa-table-cells-large',
    },
    {
      label: this.lang()== 'en'? 'Users' : 'المستخدمين',
      route: '/users',
      icon: 'fa-solid fa-users',
    },
    {
      label: this.lang()== 'en'? 'Owners' : 'المستأجرين',
      route: '/owners',
      icon: 'fa-solid fa-user-tie',
    },
    {
      label: this.lang()== 'en'? 'Properties' : 'العقارات'  ,
      route: '/properties',
      icon: 'fa-solid fa-building',
    },
    {
      label: this.lang()== 'en'? 'Tenants' : 'الملاك'  ,
      route: '/tenants',
      icon: 'fa-solid fa-users',
      badge: 3,
    },
    {
      label: this.lang()== 'en'? 'Contracts' : 'العقود',
      route: '/contracts',
      icon: 'fa-solid fa-file-signature',
    },
    {
      label: this.lang()== 'en'? 'Payments' : 'المدفوعات',
      route: '/payments',
      icon: 'fa-regular fa-credit-card',
    },
    {
      label: this.lang()== 'en'? 'Reports' : 'التقارير',
      route: '/reports',
      icon: 'fa-solid fa-chart-column',
    },
    {
      label: this.lang()== 'en'? 'Settings' : 'الإعدادات',
      route: '/settings',
      icon: 'fa-solid fa-gear',
    },
  ];
 
   toggle(): void {
    this.collapsed.update(v => !v);
    this.collapseChange.emit(this.collapsed());
  }
}
