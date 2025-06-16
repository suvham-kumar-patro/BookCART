import { ViewChild, ElementRef } from '@angular/core';
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
import { ToastrService } from 'ngx-toastr';

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

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  constructor(private fb: FormBuilder, private bookService: BookService, private toastr: ToastrService) {
  this.bookForm = this.fb.group({
  title: ['', Validators.required],
  author: ['', Validators.required],
  publisher: ['', Validators.required],      
  language: ['', Validators.required],
  format: ['Paperback', Validators.required],
  condition: ['', Validators.required],
  category: ['', Validators.required],
  customCategory: [''],
  stream: [''],                              
  exam: [''],                                
  subject: [''],                             
  publicationYear: [new Date().getFullYear(), [Validators.required, Validators.min(0)]],
  price: [0, [Validators.required, Validators.min(1)]],
  description: ['', Validators.required]
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
  formData.append('Title', this.bookForm.value.title);
  formData.append('Author', this.bookForm.value.author);
  formData.append('Language', this.bookForm.value.language);
  formData.append('Format', this.bookForm.value.format);
  formData.append('Condition', this.bookForm.value.condition);
  formData.append('Category', finalCategory);
  formData.append('PublicationYear', this.bookForm.value.publicationYear.toString());
  formData.append('Price', this.bookForm.value.price.toString());
  formData.append('Description', this.bookForm.value.description);
  formData.append('Publisher', this.bookForm.value.publisher);
  formData.append('Stream', this.bookForm.value.stream);
  formData.append('Exam', this.bookForm.value.exam);
  formData.append('Subject', this.bookForm.value.subject);


  if (this.selectedImage) {
    formData.append('Image', this.selectedImage); 
  }
  
  this.bookService.addBook(formData).subscribe({
    next: () => {
      this.toastr.success('Book submitted successfully!', 'Success');
      this.bookForm.reset();
      this.fileInput.nativeElement.value = '';
    },
    
    error: (err) => {
      console.error(err);
      this.toastr.error('Failed to submit book', 'Error');
    }
  });
}
}