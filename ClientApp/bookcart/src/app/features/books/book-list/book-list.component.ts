import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Book } from '../../../core/models/book';
import { BookService } from '../../../core/services/book.service';
import { CartService } from '../../../core/services/cart.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];

  constructor(private bookService: BookService, private cartService: CartService, private toastr: ToastrService) {}
  
  ngOnInit() {
    this.bookService.getBooks().subscribe({
      next: (data) => this.books = data,
      error: (err) => {
        console.error('Error loading books', err);
        this.toastr.error('Failed to load books.', 'Error');
      }
    });
  }

  addToCart(book: Book): void {
    this.cartService.addToCart(book);
    this.toastr.success(`"${book.title}" added to cart!`, 'Added to Cart');
  }
}
