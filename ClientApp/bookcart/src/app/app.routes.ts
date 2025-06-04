import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { BookListComponent } from './features/books/book-list/book-list.component';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { BookDetailComponent } from './features/books/book-detail/book-detail.component';
import { CartComponent } from './features/cart/cart.component';
import { SellBookComponent } from './features/sell/sell-book/sell-book.component';
import { OrdersComponent } from './features/orders/orders.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'books', component: BookListComponent },
  { path: 'books/:id', component: BookDetailComponent },
  { path: 'sell', component: SellBookComponent },
  { path: 'cart', component: CartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'orders', component: OrdersComponent}
];
