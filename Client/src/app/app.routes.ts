import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { MainPage } from './pages/main-page/main-page';
import { authGuard, notAuthGuard } from './core/guards/auth-guard';
import { UsersLit } from './pages/user/users-lit/users-lit';

export const routes: Routes = [
  { path: 'register', component: Register, canActivate: [notAuthGuard] },
  { path: 'login', component: Login, canActivate: [notAuthGuard] },
  { path: '', component: MainPage, canActivate: [authGuard] },

  // user routes
  { path: 'users', component: UsersLit, canActivate: [authGuard] }

];
