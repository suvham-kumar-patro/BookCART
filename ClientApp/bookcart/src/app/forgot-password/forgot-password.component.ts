import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  email: string = '';
  message: string = '';
  error: string = '';
  loading = false;

  constructor(private authService: AuthService) {}

  onSubmit() {
    if (this.loading) return;

    this.loading = true;
    this.message = '';
    this.error = '';

    this.authService.forgotPassword(this.email, { responseType: 'text' }).subscribe({
      next: () => {
        this.message = 'If an account with that email exists, a reset link has been sent.';
        this.loading = false;
      },
      error: () => {
        this.error = 'Something went wrong. Please try again later.';
        this.loading = false;
      }
    });
  }
}