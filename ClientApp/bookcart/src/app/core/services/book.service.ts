import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = 'https://localhost:7231/api/Books';

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }
  
  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  getBookById(id: number): Observable<Book> {
  return this.http.get<Book>(`${this.apiUrl}/${id}`);
}

filterBooks(filters: any): Observable<Book[]> {
  let params = new HttpParams();

  if (filters.search) params = params.set('search', filters.search);

  if (filters.mainCategory) params = params.set('mainCategory', filters.mainCategory);
  if (filters.schoolClass) params = params.set('schoolClass', filters.schoolClass);
  if (filters.board) params = params.set('board', filters.board);
  if (filters.collegeLevel) params = params.set('collegeLevel', filters.collegeLevel);
  if (filters.stream) params = params.set('stream', filters.stream);
  if (filters.course) params = params.set('course', filters.course);
  if (filters.honors) params = params.set('honors', filters.honors);
  if (filters.medicalCourse) params = params.set('medicalCourse', filters.medicalCourse);
  if (filters.othersCategory) params = params.set('othersCategory', filters.othersCategory);

  if (filters.minPrice != null) params = params.set('minPrice', filters.minPrice.toString());
  if (filters.maxPrice != null) params = params.set('maxPrice', filters.maxPrice.toString());
  if (filters.format) params = params.set('format', filters.format);

  return this.http.get<Book[]>(`https://localhost:7231/api/Books/filter`, { params });
}

addBook(formData: FormData): Observable<any> {
  return this.http.post(`${this.apiUrl}/sell`, formData);
}

updateBook(id: number, bookData: FormData): Observable<any> {
  return this.http.put(`${this.apiUrl}/${id}`, bookData);
}

updateBookWithFile(id: number, formData: FormData) {
  return this.http.put(`${this.apiUrl}/upload/${id}`, formData);
}

deleteBook(id: number): Observable<any> {
  return this.http.delete(`${this.apiUrl}/${id}`);
}
}
