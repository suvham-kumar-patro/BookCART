using BookCARTWebApi.DTOs;
using BookCARTWebApi.Models;

namespace BookCARTWebApi.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetApprovedBooksAsync();
        Task<IEnumerable<Book>> GetPendingApprovalAsync();
        Task<IEnumerable<BookWithUserDto>> GetApprovedBooksWithUserDtoAsync();

        Task<IEnumerable<BookWithUserDto>> GetPendingApprovalWithUserAsync();
        Task UpdateBookAsync(int id, BookUpdateDto dto);
        Task<List<BookWithUserDto>> FilterBooksAsync(
            string? search,
            string? mainCategory,
            string? schoolClass,
            string? board,
            string? collegeLevel,
            string? stream,
            string? course,
            string? honors,
            string? medicalCourse,
            string? othersCategory,
            decimal? minPrice,
            decimal? maxPrice,
            string? format);

        //Task<List<BookWithUserDto>> FilterBooksAsync(string? search, string? category, decimal? minPrice, decimal? maxPrice, string? format);
        Task DeleteBookAsync(int id);
        Task<Book?> GetBookByIdAsync(int id);
        void Update(Book book); // for admin use
        Task SaveAsync();
    }
}
