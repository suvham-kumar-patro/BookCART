import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Book } from '../models/book';

export interface CartItem {
  id: number;
  book: Book;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = new BehaviorSubject<CartItem[]>([]);
  cart$ = this.cartItems.asObservable();

  private readonly apiUrl = 'https://localhost:44309/api/Cart';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  private mapCartItem(item: any): CartItem {
    return {
      id: item.id,
      quantity: item.quantity,
      book: {
        id: item.bookId,
        title: item.title,
        imageUrl: item.imageUrl,
        price: item.price,
        description: item.description || '',
        author: item.author || '',
        publisher: item.publisher || '',
        category: item.category || '',
        format: item.format || '',
        language: item.language || '',
        condition: item.condition || '',
        publicationYear: item.publicationYear || 0,
        isApproved: true,
        userId: 0
      }
    };
  }

  loadCartFromServer() {
    this.http.get<any[]>(this.apiUrl, { headers: this.getAuthHeaders() }).pipe(
      map(items => items.map(item => this.mapCartItem(item)))
    ).subscribe({
      next: (mappedItems) => this.cartItems.next(mappedItems),
      error: (err) => console.error('Failed to load cart', err)
    });
  }

  getCartItems(): Observable<CartItem[]> {
    return this.http.get<any[]>(`${this.apiUrl}/CartItem`, { headers: this.getAuthHeaders() }).pipe(
      map(items => items.map(item => this.mapCartItem(item)))
    );
  }

  addToCart(book: Book) {
    const body = { bookId: book.id, quantity: 1, price: book.price };
    this.http.post(`${this.apiUrl}/add`, body, { headers: this.getAuthHeaders() }).subscribe({
      next: () => this.loadCartFromServer(),
      error: (err) => console.error('Failed to add to cart', err)
    });
  }

  decreaseQuantity(bookId: number) {
  const currentItem = this.cartItems.value.find(item => item.book.id === bookId);
  if (currentItem) {
    if (currentItem.quantity > 1) {
      const body = {
        bookId: bookId,
        quantity: currentItem.quantity - 1
      };
      this.http.put(`${this.apiUrl}/update`, body, { headers: this.getAuthHeaders() }).subscribe(() => {
        this.loadCartFromServer();
      });
    } else {
      // Quantity = 1, so remove item
      this.removeFromCart(currentItem.id ?? 0);
    }
  }
}


  // decreaseQuantity(bookId: number) {
  //   const currentItem = this.cartItems.value.find(item => item.book.id === bookId);
  //   if (currentItem && currentItem.quantity > 1) {
  //     const body = { bookId, quantity: currentItem.quantity - 1 };
  //     this.http.post(`${this.apiUrl}/add`, body, { headers: this.getAuthHeaders() }).subscribe(() => this.loadCartFromServer());
  //   } else {
  //     this.removeFromCart(currentItem?.id ?? 0); // fallback in case id is undefined
  //   }
  // }

  removeFromCart(cartItemId: number) {
    if (!cartItemId) {
    console.error('Cart item ID is undefined!');
    return;
  }
  this.http.delete(`${this.apiUrl}/${cartItemId}`, { headers: this.getAuthHeaders() }).subscribe({
    next: () => this.loadCartFromServer(),
    error: (err) => console.error('Failed to remove from cart', err)
  });
}

clearCart() {
  this.http.delete(`${this.apiUrl}/clear`, { headers: this.getAuthHeaders() }).subscribe(() => {
    this.cartItems.next([]);
  });
}

  getTotalPrice(): number {
    return this.cartItems.value.reduce((total, item) => total + item.book.price * item.quantity, 0);
  }

  cartCount$ = this.cart$.pipe(
    map(items => items.reduce((total, item) => total + item.quantity, 0))
  );

  placeOrder(): Observable<any> {
    return this.http.post('https://localhost:44309/api/Orders/place', {}, { headers: this.getAuthHeaders() }).pipe(
      tap(() => this.clearCart())
    );
  }
}

