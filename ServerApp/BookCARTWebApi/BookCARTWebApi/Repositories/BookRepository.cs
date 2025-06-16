using BookCARTWebApi.Data;
using BookCARTWebApi.DTOs;
using BookCARTWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCARTWebApi.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Book>> GetApprovedBooksAsync()
            => await _context.Books.Where(b => b.IsApproved)
            .ToListAsync();

        public async Task<IEnumerable<Book>> GetPendingApprovalAsync()
            => await _context.Books.Where(b => !b.IsApproved).ToListAsync();

        public async Task<IEnumerable<Book>> GetAllApprovedBooksWithUserAsync()
        {
            return await _context.Books
                .Where(b => b.IsApproved)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookWithUserDto>> GetApprovedBooksWithUserDtoAsync()
        {
            return await _context.Books
                .Where(b => b.IsApproved)
                .Include(b => b.User)
                .Select(b => new BookWithUserDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Language = b.Language,
                    Format = b.Format,
                    IsApproved = b.IsApproved,
                    Description = b.Description,
                    Category = b.Category,
                    PublicationYear = b.PublicationYear,
                    Price = b.Price,
                    UserName = b.User != null ? b.User.Username : "unknown",
                    PhoneNumber = b.User != null ? b.User.PhoneNumber : "",

                    ImageUrl = b.ImageUrl,     
                    Condition = b.Condition

                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookWithUserDto>> GetPendingApprovalWithUserAsync()
        {
            return await _context.Books
                .Where(b => !b.IsApproved)
                .Include(b => b.User)
                .Select(b => new BookWithUserDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    Author = b.Author,
                    Language = b.Language,
                    Condition = b.Condition,
                    Format = b.Format,
                    IsApproved = b.IsApproved,
                    Description = b.Description,
                    Category = b.Category,
                    PublicationYear = b.PublicationYear,
                    Price = b.Price,
                    UserName = b.User != null ? b.User.Username : "Unknown",
                    PhoneNumber = b.User != null ? b.User.PhoneNumber : ""
                })
                .ToListAsync();
        }

        public async Task UpdateBookAsync(int id, BookUpdateDto dto)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null) return;

            if (!string.IsNullOrWhiteSpace(dto.Title)) existingBook.Title = dto.Title;
            if (!string.IsNullOrWhiteSpace(dto.Author)) existingBook.Author = dto.Author;
            if (!string.IsNullOrWhiteSpace(dto.Description)) existingBook.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.Category)) existingBook.Category = dto.Category;
            if (!string.IsNullOrWhiteSpace(dto.Language)) existingBook.Language = dto.Language;
            if (!string.IsNullOrWhiteSpace(dto.Format)) existingBook.Format = dto.Format;
            if (!string.IsNullOrWhiteSpace(dto.Condition)) existingBook.Condition = dto.Condition;

            if (dto.PublicationYear.HasValue) existingBook.PublicationYear = dto.PublicationYear.Value;
            if (dto.Price.HasValue) existingBook.Price = dto.Price.Value;
            if (dto.IsApproved.HasValue) existingBook.IsApproved = dto.IsApproved.Value;

            await _context.SaveChangesAsync();
        }




        //public async Task UpdateBookAsync(int id, BookUpdateDto dto)
        //{
        //    var existingBook = await _context.Books.FindAsync(id);
        //    if (existingBook == null) return;

        //    existingBook.Title = dto.Title;
        //    existingBook.Author = dto.Author;
        //    existingBook.Description = dto.Description;
        //    existingBook.Category = dto.Category;
        //    existingBook.Language = dto.Language;
        //    existingBook.Format = dto.Format;
        //    existingBook.Price = dto.Price;
        //    existingBook.Condition = dto.Condition;
        //    existingBook.PublicationYear = dto.PublicationYear;
        //    existingBook.IsApproved = dto.IsApproved;

        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        } 

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        //public async Task<List<BookWithUserDto>> FilterBooksAsync(string? search, string? category, decimal? minPrice, decimal? maxPrice, string? format)
        //{
        //    var query = _context.Books
        //        .Include(b => b.User)
        //        .Where(b => b.IsApproved)
        //        .AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(search))
        //        query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));

        //    if (!string.IsNullOrWhiteSpace(category))
        //    {
        //        string loweredCategory = category.ToLower();
        //        query = query.Where(b => b.Category.ToLower() == loweredCategory);
        //    }

        //if (!string.IsNullOrWhiteSpace(category))
        //    query = query.Where(b => b.Category == category);

        //    if (minPrice.HasValue)
        //        query = query.Where(b => b.Price >= (double)minPrice.Value);

        //    if (maxPrice.HasValue)
        //        query = query.Where(b => b.Price <= (double)maxPrice.Value);

        //    if (!string.IsNullOrWhiteSpace(format))
        //        query = query.Where(b => b.Format == format);

        //    return await query.Select(b => new BookWithUserDto
        //    {
        //        Id = b.Id,
        //        Title = b.Title,
        //        Author = b.Author,
        //        Description = b.Description,
        //        Category = b.Category,
        //        Language = b.Language,
        //        Format = b.Format,
        //        Price = b.Price,
        //        ImageUrl = b.ImageUrl,
        //        Condition = b.Condition,
        //        PublicationYear = b.PublicationYear,
        //        IsApproved = b.IsApproved,
        //        UserName = b.User.Username,
        //        PhoneNumber = b.User.PhoneNumber
        //    }).ToListAsync();
        //}

    public async Task<List<BookWithUserDto>> FilterBooksAsync(
        string? search,
        string? category,
        decimal? minPrice,
        decimal? maxPrice,
        string? format)
        {
            var query = _context.Books
                .Include(b => b.User)
                .Where(b => b.IsApproved)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string loweredSearch = search.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(loweredSearch) ||
                    b.Author.ToLower().Contains(loweredSearch));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                string loweredCategory = category.Trim().ToLower();
                query = query.Where(b =>
                    b.Category != null && b.Category.Trim().ToLower().Contains(loweredCategory));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= (double)minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= (double)maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(format))
            {
                string loweredFormat = format.ToLower();
                query = query.Where(b => b.Format.ToLower() == loweredFormat);
            }

            return await query.Select(b => new BookWithUserDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Category = b.Category,
                Language = b.Language,
                Format = b.Format,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                Condition = b.Condition,
                PublicationYear = b.PublicationYear,
                IsApproved = b.IsApproved,
                UserName = b.User.Username,
                PhoneNumber = b.User.PhoneNumber
            }).ToListAsync();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
