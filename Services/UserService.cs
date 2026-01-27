using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;

    public UserService(IUserRepository userRepository, IBookingRepository bookingRepository)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !user.IsActive)
        {
            return null;
        }

        // Verify password (in production, use proper hashing like BCrypt)
        if (!VerifyPassword(password, user.Password))
        {
            return null;
        }

        return user;
    }

    public async Task<User> RegisterAsync(string email, string password, string firstName, string lastName, string? phone)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            throw new ArgumentException("Invalid email format");
        }

        // Check if email already exists
        if (await _userRepository.IsEmailExistsAsync(email))
        {
            throw new ArgumentException("Email already registered");
        }

        // Validate password strength
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters");
        }

        var user = new User
        {
            Email = email,
            Password = HashPassword(password), // In production, use BCrypt
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phone,
            Role = "Customer",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        return await _userRepository.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User> UpdateProfileAsync(int userId, string firstName, string lastName, string? phone, string? address)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phone;
        user.Address = address;
        user.UpdatedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        if (!VerifyPassword(currentPassword, user.Password))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            throw new ArgumentException("New password must be at least 6 characters");
        }

        user.Password = HashPassword(newPassword);
        user.UpdatedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _bookingRepository.GetUserBookingsAsync(userId);
    }

    // Password hashing helpers
    // TODO: In production, use BCrypt or similar secure hashing
    private string HashPassword(string password)
    {
        // Simple placeholder - replace with BCrypt in production
        return password;
    }

    private bool VerifyPassword(string inputPassword, string storedPassword)
    {
        // Simple placeholder - replace with BCrypt verification in production
        return inputPassword == storedPassword;
    }
}
