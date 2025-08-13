import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  password = '';
  confirmPassword = '';
  message = '';
  token = '';
  userId = '';
  showPassword = false;
  showConfirmPassword = false;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService 
  ) {}

  togglePasswordVisibility(field: 'password' | 'confirmPassword') {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      this.userId = params['userId'] || '';
    });
  }

  onSubmit() {
  if (this.password !== this.confirmPassword) {
    this.toastr.error('Passwords do not match.');
    return;
  }

  this.authService.resetPassword({
    userId: Number(this.userId),
    token: this.token,
    newPassword: this.password
  })
  .subscribe({
    next: () => {
      this.toastr.success('Password successfully reset.');
      setTimeout(() => this.router.navigate(['/login']), 2000);
    },
    error: (err) => {
      let errorMessage = 'Something went wrong.';

      if (err.error) {
        if (typeof err.error === 'string') {
          errorMessage = err.error;
        } else if (err.error.message) {
          errorMessage = err.error.message;
        } else {
          errorMessage = JSON.stringify(err.error);
        }
      }

      this.toastr.error(errorMessage);
    }
  });
}
}
