import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    if (!this.username || !this.password) {
      this.error = 'Username and Password are required';
      return;
    }

    this.authService.login({ username: this.username, password: this.password }).subscribe({
  next: (res) => {
    localStorage.setItem('token', res.token);
    this.router.navigate(['/books']);
  },
  error: () => this.error = 'Invalid credentials'
});
  }
}
