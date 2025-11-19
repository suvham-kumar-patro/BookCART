import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../../core/services/book.service';
import { Book } from '../../../core/models/book';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-book',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.scss']
})
export class EditBookComponent implements OnInit {

  editForm!: FormGroup;
  bookId!: number;
  selectedFile: File | null = null;
  preview: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private bookService: BookService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.bookId = Number(this.route.snapshot.paramMap.get('id'));

    this.editForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      author: ['', Validators.required],
      price: ['', Validators.required],
      category: ['', Validators.required],
      format: ['', Validators.required],
      language: ['', Validators.required],
      condition: ['', Validators.required],
      publicationYear: ['', Validators.required],
      imageUrl: ['']
    });

    this.loadBook();
  }

  loadBook() {
    this.bookService.getBookById(this.bookId).subscribe((book: Book) => {
      this.editForm.patchValue({
        title: book.title,
        description: book.description,
        author: book.author,
        price: book.price,
        category: book.category,
        format: book.format,
        language: book.language,
        condition: book.condition,
        publicationYear: book.publicationYear,
        imageUrl: book.imageUrl
      });

      this.preview =  'https://localhost:7231' + book.imageUrl;
    });
  }

  onFileSelected(event: any) {
  const file = event.target.files[0];

  if (file) {
    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.preview = reader.result;
    };
    reader.readAsDataURL(file);
  }
}

onSubmit() {
  if (this.editForm.invalid) return;

  const formData = new FormData();

  formData.append("title", this.editForm.get("title")?.value);
  formData.append("author", this.editForm.get("author")?.value);
  formData.append("description", this.editForm.get("description")?.value);
  formData.append("category", this.editForm.get("category")?.value);
  formData.append("stream", this.editForm.get("stream")?.value || "");
  formData.append("exam", this.editForm.get("exam")?.value || "");
  formData.append("subject", this.editForm.get("subject")?.value || "");
  formData.append("language", this.editForm.get("language")?.value);
  formData.append("format", this.editForm.get("format")?.value);
  formData.append("condition", this.editForm.get("condition")?.value);
  formData.append("publicationYear", this.editForm.get("publicationYear")?.value);
  formData.append("price", this.editForm.get("price")?.value);

  if (this.selectedFile) {
    formData.append("Image", this.selectedFile);
  }

  this.bookService.updateBook(this.bookId, formData).subscribe(() => {
    alert("Book updated successfully!");
    this.router.navigate(['/books']);
  });
}
}
