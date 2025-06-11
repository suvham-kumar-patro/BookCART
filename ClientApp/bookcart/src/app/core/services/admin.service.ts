import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CartItemAdminDto } from '../models/cart-item-admin.dto';
import { CartItemDto } from '../models/cart-item.dto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private adminApi = 'https://localhost:44309/api/admin';
  private cartApi = 'https://localhost:44309/api/cart';

  constructor(private http: HttpClient) {}

  // Existing admin endpoints
  getPendingBooks(): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminApi}/pending`);
  }

  approveBook(id: number): Observable<any> {
    return this.http.post(`${this.adminApi}/approve/${id}`, {});
  }

  rejectBook(id: number): Observable<any> {
    return this.http.delete(`${this.adminApi}/reject/${id}`);
  }

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminApi}/users`);
  }

  getOrders(): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminApi}/orders`);
  }

  // 👇 New Cart Admin Methods
  getAllCartItems(): Observable<CartItemAdminDto[]> {
    return this.http.get<CartItemAdminDto[]>(`${this.cartApi}/all`);
  }

  getCartItemsByUserId(userId: number): Observable<CartItemDto[]> {
    return this.http.get<CartItemDto[]>(`${this.cartApi}/${userId}`);
  }
}
