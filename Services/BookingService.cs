using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Booking> CreateBookingAsync(int userId, int roomId, DateTime checkIn, DateTime checkOut, int guestCount, string? specialRequests = null)
    {
        // Validate dates
        if (checkOut <= checkIn)
        {
            throw new ArgumentException("Check-out date must be after check-in date");
        }

        if (checkIn.Date < DateTime.Today)
        {
            throw new ArgumentException("Check-in date cannot be in the past");
        }

        // Check room availability
        if (!await IsRoomAvailableAsync(roomId, checkIn, checkOut))
        {
            throw new InvalidOperationException("Room is not available for the selected dates");
        }

        // Get room details for pricing
        var room = await _roomRepository.GetRoomWithDetailsAsync(roomId);
        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        // Validate guest count
        if (guestCount > room.RoomType.Capacity)
        {
            throw new ArgumentException($"Guest count exceeds room capacity of {room.RoomType.Capacity}");
        }

        // Calculate total price
        var totalPrice = await CalculateTotalPriceAsync(roomId, checkIn, checkOut);

        var booking = new Booking
        {
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            GuestCount = guestCount,
            TotalPrice = totalPrice,
            Status = "Pending",
            SpecialRequests = specialRequests,
            CreatedDate = DateTime.UtcNow
        };

        return await _bookingRepository.AddAsync(booking);
    }

    public async Task<Booking?> GetBookingDetailsAsync(int bookingId)
    {
        return await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
    {
        return await _bookingRepository.GetUserBookingsAsync(userId);
    }

    public async Task<IEnumerable<Booking>> GetHotelBookingsAsync(int hotelId)
    {
        return await _bookingRepository.GetHotelBookingsAsync(hotelId);
    }

    public async Task<Booking> ConfirmBookingAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        if (booking.Status != "Pending")
        {
            throw new InvalidOperationException($"Cannot confirm booking with status '{booking.Status}'");
        }

        booking.Status = "Confirmed";
        booking.UpdatedDate = DateTime.UtcNow;

        await _bookingRepository.UpdateAsync(booking);
        return booking;
    }

    public async Task<Booking> CheckInAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        if (booking.Status != "Confirmed")
        {
            throw new InvalidOperationException($"Cannot check-in booking with status '{booking.Status}'");
        }

        // Update booking status
        booking.Status = "CheckedIn";
        booking.UpdatedDate = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);

        // Update room status
        await _roomRepository.UpdateRoomStatusAsync(booking.RoomId, "Occupied");

        return booking;
    }

    public async Task<Booking> CheckOutAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        if (booking.Status != "CheckedIn")
        {
            throw new InvalidOperationException($"Cannot check-out booking with status '{booking.Status}'");
        }

        // Update booking status
        booking.Status = "CheckedOut";
        booking.UpdatedDate = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);

        // Update room status back to Available
        await _roomRepository.UpdateRoomStatusAsync(booking.RoomId, "Available");

        return booking;
    }

    public async Task<Booking> CancelBookingAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        var cancellableStatuses = new[] { "Pending", "Confirmed" };
        if (!cancellableStatuses.Contains(booking.Status))
        {
            throw new InvalidOperationException($"Cannot cancel booking with status '{booking.Status}'");
        }

        booking.Status = "Cancelled";
        booking.UpdatedDate = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking);

        // If room was occupied, free it
        if (booking.Room.Status == "Occupied")
        {
            await _roomRepository.UpdateRoomStatusAsync(booking.RoomId, "Available");
        }

        return booking;
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
    {
        return await _bookingRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut, excludeBookingId);
    }

    public async Task<decimal> CalculateTotalPriceAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var room = await _roomRepository.GetRoomWithDetailsAsync(roomId);
        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        int nights = (int)(checkOut - checkIn).TotalDays;
        if (nights <= 0)
        {
            throw new ArgumentException("Invalid date range");
        }

        return nights * room.RoomType.BasePrice;
    }

    public async Task<IEnumerable<Booking>> GetBookingsForDateRangeAsync(int hotelId, DateTime startDate, DateTime endDate)
    {
        return await _bookingRepository.GetBookingsForDateRangeAsync(hotelId, startDate, endDate);
    }
}
