using DataAccess.Models;

namespace Services.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateBookingAsync(int userId, int roomId, DateTime checkIn, DateTime checkOut, int guestCount, string? specialRequests = null);
    Task<Booking?> GetBookingDetailsAsync(int bookingId);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
    Task<IEnumerable<Booking>> GetHotelBookingsAsync(int hotelId);
    
    // Status operations
    Task<Booking> ConfirmBookingAsync(int bookingId);
    Task<Booking> CheckInAsync(int bookingId);
    Task<Booking> CheckOutAsync(int bookingId);
    Task<Booking> CancelBookingAsync(int bookingId);
    
    // Availability
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
    Task<decimal> CalculateTotalPriceAsync(int roomId, DateTime checkIn, DateTime checkOut);
    
    // Calendar
    Task<IEnumerable<Booking>> GetBookingsForDateRangeAsync(int hotelId, DateTime startDate, DateTime endDate);
}
