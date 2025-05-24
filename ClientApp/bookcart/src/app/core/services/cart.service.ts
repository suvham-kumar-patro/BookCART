import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Book } from '../models/book';

export interface CartItem {
  book: Book;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = new BehaviorSubject<CartItem[]>([]);
  cart$ = this.cartItems.asObservable();

  addToCart(book: Book) {
    const items = this.cartItems.value;
    const existing = items.find(item => item.book.id === book.id);

    if (existing) {
      existing.quantity += 1;
      this.cartItems.next([...items]);
    } else {
      this.cartItems.next([...items, { book, quantity: 1 }]);
    }
  }

  cartCount$ = this.cart$.pipe(
    map(items => items.reduce((total, item) => total + item.quantity, 0))
  );

  getCartItems(): CartItem[] {
    return this.cartItems.value;
  }

  decreaseQuantity(bookId: number) {
    const items = this.cartItems.value.map(item => {
      if (item.book.id === bookId && item.quantity > 1) {
        return { ...item, quantity: item.quantity - 1 };
      }
      return item;
    }).filter(item => item.quantity > 0); // remove if quantity hits 0

    this.cartItems.next(items);
  }

  removeFromCart(bookId: number) {
    const updated = this.cartItems.value.filter(item => item.book.id !== bookId);
    this.cartItems.next(updated);
  }

  clearCart() {
    this.cartItems.next([]);
  }

  getTotalPrice(): number {
    return this.cartItems.value.reduce((total, item) => total + item.book.price * item.quantity, 0);
  }
}
