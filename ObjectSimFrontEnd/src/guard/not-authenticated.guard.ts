import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const notAuthenticatedGuard: CanActivateFn = (route, state) => {
    const isAuthenticated = localStorage.getItem('key') !== null;

    console.log(isAuthenticated, "isAuthenticated guard executed");
    if (isAuthenticated) {
        const router = inject(Router);
        return router.parseUrl('/');
    }

    return true;
}