import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Book } from '../../../core/models/book';
import { BookFilter } from '../../../core/models/book-filter';
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
  filters: BookFilter = {
    search: '',
    mainCategory: '',
    schoolClass: '',
    board: '',
    collegeLevel: '',
    stream: '',
    course: '',
    honors: '',
    medicalCourse: '',
    othersCategory: '',
    format: ''
  };

  schoolClasses: string[] = ['6', '7', '8', '9', '10'];
  boards: string[] = ['CBSE', 'ICSE', 'CHSE'];
  collegeLevels: string[] = ['+2'];
  streams: string[] = ['Science', 'Commerce', 'Arts'];
  courses: string[] = ['BSc', 'BCom', 'BA'];
  honors: string[] = [
    'Mathematics', 'Chemistry', 'Physics', 'Botany', 'Zoology', 'IT',
    'English', 'History', 'Political Science', 'Economics', 'Sociology',
    'Psychology', 'Geography', 'Odia'
  ];
  medicalCourses: string[] = ['MBBS', 'BDS', 'Nursing', 'Pharmacy', 'Physiotherapy'];
  othersCategories: string[] = ['General Knowledge', 'Competitive Exams', 'Literature'];

  constructor(
    private bookService: BookService,
    private cartService: CartService,
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.filterBooks(this.filters).subscribe({
      next: (res) => (this.books = res),
      error: (err) => console.error(err)
    });
  }
  clearFilters() {
  this.filters = {
    mainCategory: '',
    schoolClass: '',
    board: '',
    stream: '',
    format: '',
  };
  this.loadBooks(); 
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
