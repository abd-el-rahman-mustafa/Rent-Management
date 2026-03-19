import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  // Implement your authentication logic here
  // inject Router to navigate to login page if not authenticated
  const router = inject(Router);
  const token = localStorage.getItem('accessToken');
  if (token) {
    const tokenExpiration = localStorage.getItem('accessTokenExpires');
    if (tokenExpiration) {
      const expirationDate = new Date(tokenExpiration);
      if (expirationDate > new Date()) {
        return true; // Token is valid
      } else {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('accessTokenExpires');
        // Optionally, you can redirect the user to the login page here
        router.navigate(['/login']);
        return false; // Token has expired
      }
    } else {
      router.navigate(['/login']);
      return false; // No expiration time found
    }
  } else {
    router.navigate(['/login']);
    return false; // No token found
  }
};
