import { Book } from "./book";

export interface OrderItem {
  bookId: number;
  quantity: number;
  price: number; 
  book: Book;
}
