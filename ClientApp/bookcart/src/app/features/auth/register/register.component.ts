import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  phoneNumber = '';
  error = '';
  showPassword = false;

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

   togglePasswordVisibility() {
  this.showPassword = !this.showPassword;
}

  register() {
    const user = {
      username: this.username,
      email: this.email,
      password: this.password,
      phoneNumber: this.phoneNumber
    };

    this.authService.register(user).subscribe({
    next: (res) => {
      console.log('Registration response:', res);
      this.toastr.success('Registration successful!', 'Please confirm your email.');
      this.router.navigate(['/login']);
    },
    error: (err) => {
      console.error('Registration error:', err);
      this.toastr.error('Registration failed. Try again.', 'Error');
    }
  });
}
}
