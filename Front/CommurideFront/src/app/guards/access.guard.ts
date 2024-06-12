import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { StorageService } from '../services/storage/storage.service';


// check if route data is provided with a property defining the access of the route
// then check if requirements are met for the route, if not redirects to a proper route
export const accessGuard: CanActivateFn = async (route, state) => {
  const store = inject(StorageService);
  const router = inject(Router);

  if (route.data['requiresAuth']) {
    if (!store.isLoggedIn()) {
      router.navigateByUrl('/login');
      return false;
    }
  }
  
  if (route.data['requiresAnon']) {
    if (store.isLoggedIn()) {
      router.navigateByUrl('');
      return false;
    }
  }

  return true;
};
