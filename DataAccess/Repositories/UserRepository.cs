using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(HotelBookingContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserWithBookingsAsync(int userId)
    {
        return await _dbSet
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                        .ThenInclude(rt => rt.Hotel)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _dbSet
            .OrderByDescending(u => u.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersWithHotelsAsync(string? roleFilter = null)
    {
        var query = _dbSet
            .Include(u => u.Hotel)
            .AsQueryable();

        if (!string.IsNullOrEmpty(roleFilter))
        {
            query = query.Where(u => u.Role == roleFilter);
        }

        return await query
            .OrderByDescending(u => u.CreatedDate)
            .ToListAsync();
    }
}
