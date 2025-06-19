import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../core/services/book.service';
import { Book } from '../../../core/models/book';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CartService } from '../../../core/services/cart.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.scss'
})
export class BookDetailComponent implements OnInit {
  book: Book | null = null;

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private bookService: BookService,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.bookService.getBookById(id).subscribe({
      next: (data) => this.book = data,
      error: (err) => console.error('Error loading book:', err)
    });
  }

  addToCart(): void {
    if (!this.authService.isLoggedIn()) {
    this.toastr.warning('Please login to add books to your cart.', 'Login Required');
    return;
  }

  if (this.book) {
    this.cartService.addToCart(this.book);
    this.toastr.success(`"${this.book.title}" added to cart!`, 'Added to Cart');
  }
}

goBack(): void {
  this.router.navigate(['/books']);
}

}
