import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authenticationGuard: CanActivateFn = (route, state) => {
    const isNotAuthenticated = localStorage.getItem('key') === null;

    if(isNotAuthenticated){
        const router = inject(Router);
        return router.parseUrl('/sesion/key');
    }
    
    return true;
};