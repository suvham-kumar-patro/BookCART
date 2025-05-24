import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Book } from '../../../core/models/book';
import { BookService } from '../../../core/services/book.service';


@Component({
  selector: 'app-book-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];

  constructor(private bookService: BookService) {}
  
  ngOnInit() {
    this.bookService.getBooks().subscribe({
      next: (data) => this.books = data,
      error: (err) => console.error('Error loading books', err)
    });
  }
}
