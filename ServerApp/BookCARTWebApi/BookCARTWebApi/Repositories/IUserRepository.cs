using BookCARTWebApi.Models;

namespace BookCARTWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<bool> UserExistsAsync(string username);
        Task AddUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    }
}
