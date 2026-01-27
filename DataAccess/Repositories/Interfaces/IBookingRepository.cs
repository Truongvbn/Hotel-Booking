using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
    Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
    Task<IEnumerable<Booking>> GetHotelBookingsAsync(int hotelId);
    Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status);
    Task<IEnumerable<Booking>> GetBookingsForDateRangeAsync(int hotelId, DateTime startDate, DateTime endDate);
}
