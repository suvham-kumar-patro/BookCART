import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';

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
  showPassword = false;

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

  togglePasswordVisibility() {
  this.showPassword = !this.showPassword;
}

  login(): void {
    if (!this.username || !this.password) {
      this.toastr.warning('Username and password are required', 'Missing Fields');
      return;
    }

    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: (res) => {
        this.toastr.success('Login successful!', `Welcome ${this.username}`);
        const token = res.token;
        localStorage.setItem('token', token);

        const decoded: any = jwtDecode(token);
        const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        console.log(role);
        
        this.password = '';
        this.username = '';

        if (role === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/books']);
        }
      },
      error: () => {
        this.toastr.error('Invalid username or password', 'Login Failed');
      }
    });
  }
}