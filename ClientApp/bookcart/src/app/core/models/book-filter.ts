export interface BookFilter {
  search?: string;

  mainCategory?: string;       
  schoolClass?: string;        
  board?: string;              
  collegeLevel?: string;       
  stream?: string;             
  course?: string;             
  honors?: string;            
  medicalCourse?: string;      
  othersCategory?: string;     
  
  minPrice?: number;
  maxPrice?: number;
  format?: string;             
}
