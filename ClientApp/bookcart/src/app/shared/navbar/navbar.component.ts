import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CartService } from '../../core/services/cart.service';
import { CommonModule } from '@angular/common';
import { UserProfile } from '../../core/models/user-profile.model';
import { UserService } from '../../core/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userProfile?: UserProfile;
  cartCount: number = 0;
  constructor(public authService: AuthService, private userService: UserService, private cartService: CartService, private router: Router, private toastr: ToastrService) {}

  ngOnInit() {
  this.cartService.cartCount$.subscribe(count => {
    this.cartCount = count;
  });

  this.authService.isLoggedIn$.subscribe((loggedIn) => {
    if (loggedIn) {
      this.loadUserProfile();
    } else {
      this.userProfile = undefined;
    }
  });

  if (this.authService.isLoggedIn()) {
    this.loadUserProfile();
  }
}

  loadUserProfile() {
    this.userService.getProfile().subscribe({
      next: (data) => (this.userProfile = data),
      error: (err) => console.error('Failed to load profile:', err)
    });
  }

  logout(): void {
  this.authService.logout();

  this.router.navigate(['/']).then(() => {
    this.toastr.info('You have been logged out.', 'Logout', {
      timeOut: 4000
    });
  });
}
}
