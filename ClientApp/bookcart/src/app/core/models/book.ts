export interface Book {
  id: number;
  title: string;
  imageUrl: string;
  description: string;
  author: string;
  publisher: string;
  price: number;
  category: string;       
  format: string;         
  language: string;      
  condition: string;      
  publicationYear: number;
  isApproved?: boolean;
  userId?: number;        
}
