using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelBooking.ViewModels;
using Services.Interfaces;

namespace HotelBooking.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;

    public BookingController(IBookingService bookingService, IRoomService roomService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
    }

    // GET: /Booking/Create
    [HttpGet]
    public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut, int guests)
    {
        var room = await _roomService.GetRoomDetailsAsync(roomId);
        if (room == null)
        {
            return NotFound();
        }

        // Check availability
        var isAvailable = await _bookingService.IsRoomAvailableAsync(roomId, checkIn, checkOut);
        if (!isAvailable)
        {
            TempData["Error"] = "This room is no longer available for the selected dates.";
            return RedirectToAction("Details", "Hotel", new { id = room.RoomType.HotelId, checkIn, checkOut, guests });
        }

        var nights = (int)(checkOut - checkIn).TotalDays;
        var totalPrice = await _bookingService.CalculateTotalPriceAsync(roomId, checkIn, checkOut);

        var model = new BookingViewModel
        {
            RoomId = roomId,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            GuestCount = guests,
            HotelName = room.RoomType.Hotel.Name,
            RoomTypeName = room.RoomType.Name,
            RoomNumber = room.RoomNumber,
            Capacity = room.RoomType.Capacity,
            PricePerNight = room.RoomType.BasePrice,
            Nights = nights,
            TotalPrice = totalPrice,
            HotelImageUrl = room.RoomType.Hotel.ImageUrl
        };

        return View(model);
    }

    // POST: /Booking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Reload room details
            var room = await _roomService.GetRoomDetailsAsync(model.RoomId);
            if (room != null)
            {
                model.HotelName = room.RoomType.Hotel.Name;
                model.RoomTypeName = room.RoomType.Name;
                model.RoomNumber = room.RoomNumber;
                model.Capacity = room.RoomType.Capacity;
                model.PricePerNight = room.RoomType.BasePrice;
                model.Nights = (int)(model.CheckOutDate - model.CheckInDate).TotalDays;
                model.TotalPrice = model.PricePerNight * model.Nights;
            }
            return View(model);
        }

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var booking = await _bookingService.CreateBookingAsync(
                userId,
                model.RoomId,
                model.CheckInDate,
                model.CheckOutDate,
                model.GuestCount,
                model.SpecialRequests);

            TempData["Success"] = "Booking created successfully!";
            return RedirectToAction("Confirmation", new { id = booking.BookingId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            
            // Reload room details
            var room = await _roomService.GetRoomDetailsAsync(model.RoomId);
            if (room != null)
            {
                model.HotelName = room.RoomType.Hotel.Name;
                model.RoomTypeName = room.RoomType.Name;
                model.RoomNumber = room.RoomNumber;
                model.Capacity = room.RoomType.Capacity;
                model.PricePerNight = room.RoomType.BasePrice;
            }
            return View(model);
        }
    }

    // GET: /Booking/Confirmation/5
    [HttpGet]
    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await _bookingService.GetBookingDetailsAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        // Verify user owns this booking
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (booking.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Staff"))
        {
            return Forbid();
        }

        var model = new BookingConfirmationViewModel
        {
            BookingId = booking.BookingId,
            HotelName = booking.Room.RoomType.Hotel.Name,
            City = booking.Room.RoomType.Hotel.City ?? "Unknown City",
            RoomTypeName = booking.Room.RoomType.Name,
            RoomNumber = booking.Room.RoomNumber,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            GuestCount = booking.GuestCount,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests,
            CreatedDate = booking.CreatedDate
        };

        return View(model);
    }

    // GET: /Booking/MyBookings
    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var bookings = await _bookingService.GetUserBookingsAsync(userId);

        var model = new MyBookingsViewModel
        {
            Bookings = bookings.Select(b => new BookingItemViewModel
            {
                BookingId = b.BookingId,
                HotelName = b.Room.RoomType.Hotel.Name ?? "Unknown Hotel",
                City = b.Room.RoomType.Hotel.City ?? "Unknown City",
                RoomTypeName = b.Room.RoomType.Name,
                RoomNumber = b.Room.RoomNumber,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                GuestCount = b.GuestCount,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                HotelImageUrl = b.Room.RoomType.Hotel.ImageUrl
            })
        };

        return View(model);
    }

    // POST: /Booking/Cancel/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var booking = await _bookingService.GetBookingDetailsAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Verify user owns this booking
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (booking.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Staff"))
            {
                return Forbid();
            }

            await _bookingService.CancelBookingAsync(id);
            TempData["Success"] = "Booking cancelled successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("MyBookings");
    }
}
