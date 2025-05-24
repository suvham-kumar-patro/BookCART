import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService, CartItem } from '../../core/services/cart.service';
@Component({
  selector: 'app-cart',
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  totalPrice: number = 0;

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.cartService.cart$.subscribe((items) => {
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

  removeItem(bookId: number) {
    this.cartService.removeFromCart(bookId);
  }

  // removeItem(id: number) {
  //   this.cartService.removeFromCart(id);
  //   this.cartItems = this.cartService.getCartItems();
  //   this.totalPrice = this.cartService.getTotalPrice();
  // }

  checkout() {
    alert('Proceeding to checkout...');
  }

}
