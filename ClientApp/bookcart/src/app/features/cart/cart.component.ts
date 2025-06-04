import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CartService, CartItem } from '../../core/services/cart.service';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  totalPrice: number = 0;
  errorMessage ='';

  constructor(private cartService: CartService, private router: Router ) {}

  ngOnInit() {
    this.cartService.loadCartFromServer();
    this.cartService.cart$.subscribe((items) => {
      console.log('Cart items:', items);
      this.cartItems = items;
      this.totalPrice = this.cartService.getTotalPrice();
    });
  }

   increase(item: CartItem) {
    this.cartService.addToCart(item.book);
  }

  decrease(item: CartItem) {
    this.cartService.decreaseQuantity(item.book.id!);
  }

  removeItem(cartItemId: number) {
    this.cartService.removeFromCart(cartItemId);
  }

  checkout() {
    this.placeOrder();
  }

  placeOrder() {
    this.cartService.placeOrder().subscribe({
      next: () => {
        this.cartItems = []; 
        this.totalPrice = 0;
        this.router.navigate(['/orders']);
      },
      error: () => {
        this.errorMessage = 'Order placement failed. Please try again.';
      }
    });
  }
}
