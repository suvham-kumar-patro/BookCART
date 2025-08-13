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
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { AuthGuard } from './core/guards/auth.guard';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'books', component: BookListComponent },
  { path: 'books/:id', component: BookDetailComponent },
  { path: 'sell', component: SellBookComponent },
  { path: 'cart', component: CartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'admin', canActivate: [AuthGuard], data: { requiresAdmin: true }, component: AdminDashboardComponent },
  { path: 'orders', component: OrdersComponent},
  { path: 'profile', component: UserProfileComponent},
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent}
];
