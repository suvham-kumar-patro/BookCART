import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private baseUrl = 'https://localhost:7231/api/users';

  constructor(private http: HttpClient) {}

  getProfile() {
    return this.http.get<UserProfile>(`${this.baseUrl}/profile`);
  }

  updateProfile(data: { phoneNumber: string }) {
    return this.http.put(`${this.baseUrl}/profile`, data);
  }
}
