import { CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class LoginGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate() {
    return true;
    // if (localStorage.getItem('currentUser')) {
    //   // logged in so return true
    //   return true;
    // }

    // // not logged in so redirect to login page
    // this.router.navigate(['./']);
    // return false;
  }
}
