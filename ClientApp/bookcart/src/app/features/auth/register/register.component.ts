import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
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
    next: (res) => {
      console.log('Registration response:', res);
      this.router.navigate(['/login']);
    },
    error: (err) => {
      console.error('Registration error:', err);
      this.error = 'Registration failed. Try again.';
    }
  });
}
}
