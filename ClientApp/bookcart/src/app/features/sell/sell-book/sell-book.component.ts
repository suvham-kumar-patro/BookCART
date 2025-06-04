import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule
} from '@angular/forms';
import { BookService } from '../../../core/services/book.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sell-book',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './sell-book.component.html',
  styleUrl: './sell-book.component.scss',
})
export class SellBookComponent {
  bookForm!: FormGroup;
  selectedImage?: File;
  isOtherCategory = false;
  categoryOptions = ['Fiction', 'Non-Fiction', 'Science', 'Technology', 'History'];

  constructor(private fb: FormBuilder, private bookService: BookService) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      language: ['', Validators.required],
      format: ['Paperback', Validators.required],
      condition: ['', Validators.required],
      category: ['', Validators.required],
      customCategory: [''],
      publicationYear: [new Date().getFullYear(), [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(1)]],
      description: ['', Validators.required],
      imageUrl: ['']
    });
  }

  onCategoryChange(event: Event) {
    const selected = (event.target as HTMLSelectElement).value;
    this.isOtherCategory = selected === 'Other';

    const customCategoryControl = this.bookForm.get('customCategory');

    if (this.isOtherCategory) {
      customCategoryControl?.setValidators([Validators.required]);
    } else {
      customCategoryControl?.clearValidators();
      customCategoryControl?.setValue('');
    }
    customCategoryControl?.updateValueAndValidity();
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files?.length) {
      this.selectedImage = fileInput.files[0];
    }
  }

  submit() {
    const finalCategory = this.isOtherCategory
      ? this.bookForm.value.customCategory
      : this.bookForm.value.category;

    const formData = new FormData();
    formData.append('title', this.bookForm.value.title);
    formData.append('author', this.bookForm.value.author);
    formData.append('language', this.bookForm.value.language);
    formData.append('format', this.bookForm.value.format);
    formData.append('condition', this.bookForm.value.condition);
    formData.append('category', finalCategory);
    formData.append('publicationYear', this.bookForm.value.publicationYear.toString());
    formData.append('price', this.bookForm.value.price.toString());
    formData.append('description', this.bookForm.value.description);

    if (this.selectedImage) {
      formData.append('image', this.selectedImage);
    }

    this.bookService.addBook(formData).subscribe({
      next: () => alert('Book submitted successfully!'),
      error: () => alert('Failed to submit book')
    });
  }
}
