export interface BookDto {
  title: string;
  imageUrl: string;
  description: string;
  author: string;
  category: string;
  stream: string;
  exam: string;
  subject: string;
  language: string;
  condition: string;
  format: string;
  publicationYear: number;
  price: number;
}

export interface OrderItemDto {
  bookId: number;
  bookTitle: string;
  quantity: number;
  price: number;
}

export interface OrderDto {
  id: number;
  orderDate: string; // ISO string
  totalAmount: number;
  items: OrderItemDto[];
}

export interface UserProfile {
  username: string;
  phoneNumber: string;
  listedBooks: BookDto[];
  orderHistory: OrderDto[];
}
