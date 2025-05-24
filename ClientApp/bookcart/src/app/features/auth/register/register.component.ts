import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  username = '';
  password = '';
  phoneNumber = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    const user = {
      username: this.username,
      password: this.password,
      phoneNumber: this.phoneNumber
    };

    this.authService.register(user).subscribe({
      next: () => this.router.navigate(['/login']),
      error: () => this.error = 'Registration failed. Try again.'
    });
  }
}
