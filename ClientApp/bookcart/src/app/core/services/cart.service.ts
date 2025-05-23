import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Book } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = new BehaviorSubject<Book[]>([]);
  cart$ = this.cartItems.asObservable();

  addToCart(book: Book) {
    const current = this.cartItems.value;
    this.cartItems.next([...current, book]);
  }

  removeFromCart(bookId: number) {
    const updated = this.cartItems.value.filter(b => b.id !== bookId);
    this.cartItems.next(updated);
  }

  clearCart() {
    this.cartItems.next([]);
  }
}
