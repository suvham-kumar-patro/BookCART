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

                    ImageUrl = b.ImageUrl,     // ✅ Add this
                    Condition = b.Condition

                    //Id = b.Id,
                    //Title = b.Title,
                    //Author = b.Author,
                    //Description = b.Description,
                    //Category = b.Category,
                    //Language = b.Language,
                    //Format = b.Format,
                    //Price = b.Price,
                    //UserName = b.User != null ? b.User.Username : "Unknown",
                    //PhoneNumber = b.User != null ? b.User.PhoneNumber : ""

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
                    Author = b.Author,
                    Language = b.Language,
                    Format = b.Format,
                    IsApproved = b.IsApproved,
                    UserName = b.User != null ? b.User.Username : "Unknown",
                    PhoneNumber = b.User != null ? b.User.PhoneNumber : ""
                })
                .ToListAsync();
        }

        public async Task UpdateBookAsync(int id, BookUpdateDto dto)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null) return;

            existingBook.Title = dto.Title;
            existingBook.Author = dto.Author;
            existingBook.Description = dto.Description;
            existingBook.Category = dto.Category;
            existingBook.Language = dto.Language;
            existingBook.Format = dto.Format;
            existingBook.Price = dto.Price;
            existingBook.ImageUrl = dto.ImageUrl;
            existingBook.Condition = dto.Condition;
            existingBook.PublicationYear = dto.PublicationYear;
            existingBook.IsApproved = dto.IsApproved;

            await _context.SaveChangesAsync();
        }

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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
