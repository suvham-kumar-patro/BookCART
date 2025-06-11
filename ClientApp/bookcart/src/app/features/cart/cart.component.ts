import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CartService, CartItem } from '../../core/services/cart.service';
import { ToastrService } from 'ngx-toastr';
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

  constructor(private cartService: CartService, private router: Router, private toastr: ToastrService ) {}

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

  removeItem(cartItemId: number): void {
  const removedItem = this.cartItems.find(item => item.id === cartItemId); // fix here

  console.log('Removing item:', removedItem); 

  this.cartService.removeFromCart(cartItemId).subscribe({
    next: () => {
      if (removedItem) {
        this.toastr.info(`"${removedItem.book.title}" removed from cart.`, 'Item Removed');
      }
    },
    error: () => {
      this.toastr.error('Failed to remove item from cart.', 'Error');
    }
  });
}

  checkout() {
    this.placeOrder();
  }

  placeOrder() {
    this.cartService.placeOrder().subscribe({
      next: () => {
        this.cartItems = []; 
        this.totalPrice = 0;

        this.toastr.success('Your order has been placed successfully!', 'Order Placed');
        this.router.navigate(['/orders']);
      },
      error: () => {
        this.errorMessage = 'Order placement failed. Please try again.';
        this.toastr.error(this.errorMessage, 'Order Failed');
      }
    });
  }
}
