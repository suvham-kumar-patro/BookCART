import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {jwtDecode, JwtPayload } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

interface DecodedToken extends JwtPayload {
  role?: string;
  username?: string;
}
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7231/api/Auth';
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasValidToken());
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  login(credentials: { username: string; password: string }): Observable<{ token: string }> {
    return new Observable(observer => {
      this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials).subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          this.isLoggedInSubject.next(true);
          observer.next(res);
          observer.complete();
        },
        error: (err) => {
          observer.error(err)
        }
      });
    });
  }

  register(user: { username: string; password: string; phoneNumber: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.isLoggedInSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return this.hasValidToken();
  }

  private hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = this.getDecodedToken();
    if (!payload?.exp) return false;

    return (payload.exp * 1000) > Date.now(); 
  }

  getDecodedToken(): DecodedToken | null {
  const token = this.getToken();
  if (!token) return null;

  try {
    const decoded: any = jwtDecode(token);

    const roleClaim = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const username = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

    return {
      ...decoded,
      role: roleClaim,
      username: username
    };
  } catch (error) {
    console.error('Invalid token', error);
    return null;
  }
}

  isAdmin(): boolean {
    return this.hasRole('admin');
  }

  hasRole(expectedRole: string): boolean {
    const payload = this.getDecodedToken();
    return payload?.role?.toLowerCase() === expectedRole.toLowerCase();
  }

  getUsername(): string | null {
    const payload = this.getDecodedToken();
    return payload?.username ?? null;
  }

  forgotPassword(email: string, options?: any) {
  return this.http.post(
    `${this.apiUrl}/forgot-password`,
    { email },
    options || {}
  );
}

resetPassword(data: { userId: number, token: string, newPassword: string }) {
  return this.http.post(`${this.apiUrl}/reset-password`, data);
}
}