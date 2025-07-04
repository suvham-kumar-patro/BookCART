import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CartItemAdminDto } from '../models/cart-item-admin.dto';
import { CartItemDto } from '../models/cart-item.dto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private adminApi = 'https://localhost:7231/api/admin';
  private cartApi = 'https://localhost:7231/api/cart';

  constructor(private http: HttpClient) {}

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

  getAllCartItems(): Observable<CartItemAdminDto[]> {
    return this.http.get<CartItemAdminDto[]>(`${this.cartApi}/all`);
  }

  getCartItemsByUserId(userId: number): Observable<CartItemDto[]> {
    return this.http.get<CartItemDto[]>(`${this.cartApi}/${userId}`);
  }
}
