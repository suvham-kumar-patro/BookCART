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
  if (filters.category) params = params.set('category', filters.category);
  if (filters.format) params = params.set('format', filters.format);
  if (filters.minPrice != null) params = params.set('minPrice', filters.minPrice.toString());
  if (filters.maxPrice != null) params = params.set('maxPrice', filters.maxPrice.toString());

  return this.http.get<Book[]>(`https://localhost:7231/api/books/filter`, { params });
}


addBook(formData: FormData): Observable<any> {
  return this.http.post(`${this.apiUrl}/sell`, formData);
    // headers: this.getAuthHeaders());
}


}
