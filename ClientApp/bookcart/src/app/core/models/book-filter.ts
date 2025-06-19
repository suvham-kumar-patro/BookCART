export interface BookFilter {
  category?: string;
  stream?: string;
  exam?: string;
  subject?: string;
  author?: string;
  language?: string;
  condition?: string;
  format?: string;
  minPrice?: number;
  maxPrice?: number;
  publicationYear?: number;
  offersOnly?: boolean;
}
