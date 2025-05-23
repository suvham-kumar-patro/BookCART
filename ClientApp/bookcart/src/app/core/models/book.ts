export interface Book {
  id?: number;
  title: string;
  description: string;
  author: string;
  publisher: string;
  price: number;
  category: string;       // e.g., Academic, Entrance Exams
  format: string;         // Paperback, PDF, etc.
  language: string;       // English, Hindi, etc.
  condition: string;      // New, Used
  publicationYear: number;
  isApproved?: boolean;
  userId?: number;        // ID of seller
}
