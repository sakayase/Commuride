import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { StorageService } from '../services/storage.service';

export const accessGuard: CanActivateFn = async (route, state) => {
  console.log("guaaaaard");
  if (route.data['requiresAuth']) {
    const store = inject(StorageService);
    const router = inject(Router);
    if (!store.isLoggedIn()) {
      router.navigate(['login'])
    }
  }
  return true;
};
