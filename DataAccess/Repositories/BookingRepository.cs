using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(HotelBookingContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _dbSet
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                    .ThenInclude(rt => rt.Hotel)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
    {
        return await _dbSet
            .Include(b => b.User)
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                    .ThenInclude(rt => rt.Hotel)
            .Include(b => b.Payment)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
    {
        var query = _dbSet
            .Where(b => b.RoomId == roomId && 
                       b.Status != "Cancelled" &&
                       !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut));

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.BookingId != excludeBookingId.Value);
        }

        return !await query.AnyAsync();
    }

    public async Task<IEnumerable<Booking>> GetHotelBookingsAsync(int hotelId)
    {
        return await _dbSet
            .Include(b => b.User)
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
            .Where(b => b.Room.RoomType.HotelId == hotelId)
            .OrderByDescending(b => b.CheckInDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
    {
        return await _dbSet
            .Include(b => b.User)
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                    .ThenInclude(rt => rt.Hotel)
            .Where(b => b.Status == status)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsForDateRangeAsync(int hotelId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(b => b.User)
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
            .Where(b => b.Room.RoomType.HotelId == hotelId &&
                       b.Status != "Cancelled" &&
                       !(b.CheckOutDate <= startDate || b.CheckInDate >= endDate))
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();
    }
}
