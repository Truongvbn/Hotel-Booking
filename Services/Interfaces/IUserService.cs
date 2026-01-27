using DataAccess.Models;

namespace Services.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User> RegisterAsync(string email, string password, string firstName, string lastName, string? phone);
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByEmailAsync(string email);
    Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string? phone, string? address);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
}
