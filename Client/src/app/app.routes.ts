import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { MainPage } from './pages/main-page/main-page';
import { authGuard, notAuthGuard } from './core/guards/auth.guard';
import { UsersLit } from './pages/user/users-lit/users-lit';
import { langGuard } from './core/guards/lang.guard';
import { redirectToLangGuard } from './core/guards/redirect-to-lang.guard';


export const routes: Routes = [
  {
    path: ':lang',
    canActivate: [langGuard],
    children: [
      { path: 'register', component: Register, canActivate: [notAuthGuard] },
      { path: 'login', component: Login, canActivate: [notAuthGuard] },
      { path: '', component: MainPage, canActivate: [authGuard] },

      // user routes
      { path: 'users', component: UsersLit, canActivate: [authGuard] }
    ]
  },
  {
    path: '',
    canActivate: [redirectToLangGuard],
    children: [] // Redirect any unknown paths to the default language route
  },
  {
    path: '**',
    canActivate: [redirectToLangGuard],
    children: [] // Redirect any unknown paths to the default language route
  }
];
