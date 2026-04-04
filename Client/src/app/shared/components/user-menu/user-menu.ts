import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from '../../../core/components/base-component/base-component';
import { AuthService } from '../../../pages/auth/auth.service';

@Component({
  selector: 'app-user-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-menu.html',
})
export class UserMenuComponent extends BaseComponent {
  // isOpen is a signal that controls the visibility of the user menu
  isOpen = signal(false);


  private authService = inject(AuthService);

  toggleMenu() {
    // toggle the visibility of the user menu
    this.isOpen.update((v) => !v);
  }

  logout() {
    this.isOpen.set(false);
    this.authService.logout();
  }
}
