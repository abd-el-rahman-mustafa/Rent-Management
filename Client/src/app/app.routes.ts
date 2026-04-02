import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { MainPage } from './pages/main-page/main-page';
import { authGuard, notAuthGuard } from './core/guards/auth.guard';
import { UsersLit } from './pages/user/users-lit/users-lit';
import { langGuard } from './core/guards/lang.guard';
import { redirectToLangGuard } from './core/guards/redirect-to-lang.guard';
import { Layout } from './pages/layout/layout';


export const routes: Routes = [
  {
    path: ':lang',
    canActivate: [langGuard],
    children: [
      { path: 'register', component: Register, canActivate: [notAuthGuard] },
      { path: 'login', component: Login, canActivate: [notAuthGuard] },
      // layouted routes
      {
        path: '', component: Layout, canActivate: [authGuard],
        children: [
          // main page
          { path: 'dashboard', loadComponent: () => import('./pages/main-page/main-page').then(m => m.MainPage) },

          // users
          { path: 'users', loadComponent: () => import('./pages/user/users-lit/users-lit').then(m => m.UsersLit) }

        ]
      },



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
