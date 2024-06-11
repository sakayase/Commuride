import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { StorageService } from '../services/storage/storage.service';

export const accessGuard: CanActivateFn = async (route, state) => {
  if (route.data['requiresAuth']) {
    const store = inject(StorageService);
    const router = inject(Router);
    if (!store.isLoggedIn()) {
      router.navigateByUrl('/login');
      return false;
    }
  }
  return true;
};
