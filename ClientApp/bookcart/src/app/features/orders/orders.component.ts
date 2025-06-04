import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../core/services/order.service';


@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
  loading = true;
  error = '';

  constructor(private orderService: OrderService){}

  ngOnInit(): void {
    this.orderService.getMyOrders().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to fetch your orders.';
        this.loading = false;
      }
    });
  }

} 
