import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Book } from '../../../core/models/book';
import { BookService } from '../../../core/services/book.service';
import { CartService } from '../../../core/services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  filters = {
    search: '',
    category: '',
    format: '',
    minPrice: null as number | null,
    maxPrice: null as number | null
  };

  categories: string[] = ['Adventure', 'Computer Science', 'Science Fiction'];

  constructor(private bookService: BookService, private cartService: CartService, private toastr: ToastrService, private authService: AuthService) {}
  
  ngOnInit() {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.filterBooks(this.filters).subscribe({
      next: (data) => this.books = data,
      error: (err) => {
        console.error('Error loading books', err);
        this.toastr.error('Failed to load books.', 'Error');
      }
    });
  }

   onFilterChange(): void {
    this.loadBooks();
  }

  addToCart(book: Book): void {
    if (!this.authService.isLoggedIn()) {
    this.toastr.warning('Please login to add books to your cart.', 'Login Required');
    return;
  }
    this.cartService.addToCart(book);
    this.toastr.success(`"${book.title}" added to cart!`, 'Added to Cart');
  }
}
