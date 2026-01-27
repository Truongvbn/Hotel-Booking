using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories;

public class HotelRepository : Repository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelBookingContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Hotel>> GetHotelsWithRoomTypesAsync()
    {
        return await _dbSet
            .Include(h => h.RoomTypes)
            .Where(h => h.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Hotel>> SearchByCityAsync(string city)
    {
        return await _dbSet
            .Include(h => h.RoomTypes)
            .Where(h => h.City != null && h.City.ToLower().Contains(city.ToLower()) && h.IsActive)
            .ToListAsync();
    }

    public async Task<Hotel?> GetHotelWithDetailsAsync(int hotelId)
    {
        return await _dbSet
            .Include(h => h.RoomTypes)
                .ThenInclude(rt => rt.Rooms)
            .Include(h => h.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(h => h.HotelId == hotelId && h.IsActive);
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int capacity)
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .Where(r => r.RoomType.HotelId == hotelId &&
                       r.Status == "Available" &&
                       r.RoomType.Capacity >= capacity &&
                       !r.Bookings.Any(b => b.Status != "Cancelled" &&
                                           !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut)))
            .ToListAsync();
    }

    public async Task<decimal> GetAverageRatingAsync(int hotelId)
    {
        var ratings = await _context.Reviews
            .Where(r => r.HotelId == hotelId && r.IsApproved)
            .Select(r => r.Rating)
            .ToListAsync();

        return ratings.Any() ? (decimal)ratings.Average() : 0;
    }

    public async Task<IEnumerable<string>> GetAllCitiesAsync()
    {
        return await _dbSet
            .Where(h => h.IsActive && h.City != null)
            .Select(h => h.City!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
