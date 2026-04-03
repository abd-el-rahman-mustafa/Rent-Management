import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from '../../../core/components/base-component/base-component';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-menu.html',
})
export class UserMenuComponent extends BaseComponent {
  // isOpen is a signal that controls the visibility of the user menu
  isOpen = signal(false);

  toggleMenu() {
    // toggle the visibility of the user menu
    this.isOpen.update((v) => !v);
  }

  logout() {
    this.isOpen.set(false);
    // TODO: Implement actual logout logic through an auth service
    console.log('User logged out');
  }
}
