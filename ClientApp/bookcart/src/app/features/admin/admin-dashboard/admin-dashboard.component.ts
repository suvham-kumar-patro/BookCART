import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../core/services/admin.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent implements OnInit {
  view: 'pending' | 'users' | 'orders' | 'cart' = 'pending';

  pendingBooks: any[] = [];
  users: any[] = [];
  orders: any[] = [];
  cartItems: any[] = [];
  userCartItems: any[] = [];

  selectedUserId: number | null = null;

  constructor(private adminService: AdminService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadPendingBooks();
  }

  loadPendingBooks() {
    this.adminService.getPendingBooks().subscribe(data => {
      this.pendingBooks = data;
    });
  }
  
  approveBook(id: number) {
    this.adminService.approveBook(id).subscribe({
      next: () => {
        this.toastr.success('Book approved successfully!', 'Success');
        this.loadPendingBooks();
      },
      error: () => {
        this.toastr.error('Failed to approve book', 'Error');
    }
  });
}

  rejectBook(id: number) {
    this.adminService.rejectBook(id).subscribe({
      next: () => {
        this.toastr.warning('Book rejected.', 'Rejected');
        this.loadPendingBooks();
      },
      error: () => {
        this.toastr.error('Failed to reject book', 'Error');
      }
    });
  }

  loadUsers() {
    if (this.users.length === 0) {
      this.adminService.getUsers().subscribe(data => this.users = data);
    }
  }

  loadOrders() {
    if (this.orders.length === 0) {
      this.adminService.getOrders().subscribe(data => this.orders = data);
    }
  }

  loadAllCartItems() {
    this.adminService.getAllCartItems().subscribe(data => this.cartItems = data);
  }

  loadCartItemsByUser() {
    if (this.selectedUserId !== null) {
      this.adminService.getCartItemsByUserId(this.selectedUserId).subscribe(data => {
        this.userCartItems = data;
      });
    }
  }

}
