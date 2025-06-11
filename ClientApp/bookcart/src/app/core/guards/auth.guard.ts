import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const requiresAdmin = route.data['requiresAdmin'] === true;

    if (!this.authService.isLoggedIn()) {
      this.toastr.warning('Please log in to continue.');
      return this.router.createUrlTree(['/login'], {
        queryParams: { redirect: 'true' }
      });
    }

    if (requiresAdmin && !this.authService.isAdmin()) {
      this.toastr.error('Access denied. Admins only.');
      return this.router.createUrlTree(['/unauthorized']);
    }

    return true;
  }
}












// import { Injectable } from '@angular/core';
// import { CanActivate, Router, ActivatedRouteSnapshot, UrlTree } from '@angular/router';
// import { AuthService } from '../services/auth.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthGuard implements CanActivate {

//   constructor(
//     private authService: AuthService,
//     private router: Router
//   ) {}

//   canActivate(
//     route: ActivatedRouteSnapshot
//   ): boolean | UrlTree {
//     const requiresAdmin = route.data['requiresAdmin'] === true;

//     if (!this.authService.isLoggedIn()) {
//       return this.router.createUrlTree(['/login'], {
//         queryParams: { redirect: 'true' }
//       });
//     }

//     if (requiresAdmin && !this.authService.isAdmin()) {
//       return this.router.createUrlTree(['/unauthorized'], {
//         queryParams: { message: 'Access denied. Admins only.' }
//       });
//     }

//     return true;
//   }
// }
