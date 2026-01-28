using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsEmailExistsAsync(string email);
    Task<User?> GetUserWithBookingsAsync(int userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetAllUsersWithHotelsAsync(string? roleFilter = null);
}
