import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../core/services/book.service';
import { Book } from '../../../core/models/book';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-book-detail',
  imports: [CommonModule],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.scss'
})
export class BookDetailComponent implements OnInit {
  book: Book | null = null;

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private bookService: BookService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.bookService.getBookById(id).subscribe({
      next: (data) => this.book = data,
      error: (err) => console.error('Error loading book:', err)
    });
  }

  addToCart(): void {
  if (this.book) {
    this.cartService.addToCart(this.book);
    alert('Book added to cart!');
  }
}

}
