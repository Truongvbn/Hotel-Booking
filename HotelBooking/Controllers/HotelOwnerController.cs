using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelBooking.ViewModels;
using Services.Interfaces;

namespace HotelBooking.Controllers;

[Authorize(Roles = "HotelOwner")]
public class HotelOwnerController : Controller
{
    private readonly IHotelService _hotelService;
    private readonly IRoomService _roomService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public HotelOwnerController(
        IHotelService hotelService, 
        IRoomService roomService, 
        IBookingService bookingService,
        IUserService userService)
    {
        _hotelService = hotelService;
        _roomService = roomService;
        _bookingService = bookingService;
        _userService = userService;
    }

    private async Task<int?> GetUserHotelIdAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetByIdAsync(userId);
        return user?.HotelId;
    }

    // GET: /HotelOwner/Dashboard
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue)
        {
            TempData["Error"] = "You are not assigned to any hotel.";
            // Ideally redirect to a "Setup Hotel" page or contact admin
            return RedirectToAction("Index", "Home"); 
        }

        // --- Logic from AdminController.Dashboard adapted for single hotel ---
        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId.Value);
        if (hotel == null) return NotFound();

        var rooms = await _roomService.GetHotelRoomsAsync(hotelId.Value);
        var bookings = await _bookingService.GetHotelBookingsAsync(hotelId.Value);

        var today = DateTime.Today;

        var model = new DashboardViewModel
        {
            TotalHotels = 1, // Always 1 for Owner
            TotalRooms = rooms.Count(),
            AvailableRooms = rooms.Count(r => r.Status == "Available"),
            OccupiedRooms = rooms.Count(r => r.Status == "Occupied"),
            TotalBookings = bookings.Count(),
            PendingBookings = bookings.Count(b => b.Status == "Pending"),
            TodayCheckIns = bookings.Count(b => b.CheckInDate.Date == today && b.Status == "Confirmed"),
            TodayCheckOuts = bookings.Count(b => b.CheckOutDate.Date == today && b.Status == "CheckedIn"),
            TotalRevenue = bookings.Where(b => b.Status != "Cancelled").Sum(b => b.TotalPrice),
            OccupancyRate = rooms.Any() ? (decimal)rooms.Count(r => r.Status == "Occupied") / rooms.Count() * 100 : 0,
            RecentBookings = bookings
                .OrderByDescending(b => b.CreatedDate)
                .Take(5)
                .Select(b => new BookingItemViewModel
                {
                    BookingId = b.BookingId,
                    HotelName = hotel.Name,
                    RoomTypeName = b.Room.RoomType.Name,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                })
        };

        return View(model);
    }

    // GET: /HotelOwner/Hotel
    [HttpGet]
    public async Task<IActionResult> Hotel()
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        return RedirectToAction("EditHotel", new { id = hotelId.Value });
    }

    // GET: /HotelOwner/EditHotel/5
    [HttpGet]
    public async Task<IActionResult> EditHotel(int id)
    {
        var userHotelId = await GetUserHotelIdAsync();
        if (!userHotelId.HasValue || userHotelId.Value != id)
        {
            return Forbid();
        }

        var hotel = await _hotelService.GetHotelDetailsAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }

        var model = new HotelFormViewModel
        {
            HotelId = hotel.HotelId,
            Name = hotel.Name,
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            Description = hotel.Description,
            StarRating = hotel.StarRating,
            Phone = hotel.Phone,
            Website = hotel.Website,
            ImageUrl = hotel.ImageUrl,
            Latitude = hotel.Latitude,
            Longitude = hotel.Longitude
        };

        return View(model);
    }

    // POST: /HotelOwner/EditHotel
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHotel(HotelFormViewModel model)
    {
        var userHotelId = await GetUserHotelIdAsync();
        if (!userHotelId.HasValue || userHotelId.Value != model.HotelId)
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var hotel = new DataAccess.Models.Hotel
            {
                HotelId = model.HotelId,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Description = model.Description,
                StarRating = model.StarRating,
                Phone = model.Phone,
                Website = model.Website,
                ImageUrl = model.ImageUrl,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            await _hotelService.UpdateHotelAsync(hotel);
            TempData["Success"] = "Hotel updated successfully!";
            return RedirectToAction("Dashboard");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /HotelOwner/RoomTypes
    [HttpGet]
    public async Task<IActionResult> RoomTypes()
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(hotelId.Value);
        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId.Value);

        var model = new AdminRoomTypeListViewModel
        {
            HotelId = hotelId.Value,
            HotelName = hotel?.Name ?? "Unknown Hotel",
            RoomTypes = roomTypes.Select(rt => new AdminRoomTypeItemViewModel
            {
                RoomTypeId = rt.RoomTypeId,
                Name = rt.Name,
                Description = rt.Description,
                Capacity = rt.Capacity,
                BasePrice = rt.BasePrice,
                ImageUrl = rt.ImageUrl,
                RoomCount = rt.Rooms?.Count ?? 0
            })
        };

        return View(model);
    }

    // GET: /HotelOwner/CreateRoomType
    [HttpGet]
    public async Task<IActionResult> CreateRoomType()
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        var model = new RoomTypeFormViewModel
        {
            HotelId = hotelId.Value
        };
        return View(model);
    }

    // POST: /HotelOwner/CreateRoomType
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoomType(RoomTypeFormViewModel model)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue || hotelId.Value != model.HotelId) return Forbid();

        if (!ModelState.IsValid) return View(model);

        try
        {
            await _roomService.AddRoomTypeAsync(model.HotelId, model.Name, model.Description, model.Capacity, model.BasePrice);
            TempData["Success"] = "Room Type created successfully!";
            return RedirectToAction("RoomTypes");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /HotelOwner/EditRoomType/5
    [HttpGet]
    public async Task<IActionResult> EditRoomType(int id)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        // Verify ownership
        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(hotelId.Value);
        var roomType = roomTypes.FirstOrDefault(rt => rt.RoomTypeId == id);
        
        if (roomType == null) return NotFound();

        var model = new RoomTypeFormViewModel
        {
            RoomTypeId = roomType.RoomTypeId,
            HotelId = roomType.HotelId,
            Name = roomType.Name,
            Description = roomType.Description,
            Capacity = roomType.Capacity,
            BasePrice = roomType.BasePrice,
            ImageUrl = roomType.ImageUrl
        };
        return View(model);
    }

    // POST: /HotelOwner/EditRoomType
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoomType(RoomTypeFormViewModel model)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue || hotelId.Value != model.HotelId) return Forbid();

        if (!ModelState.IsValid) return View(model);

        try
        {
            var roomType = new DataAccess.Models.RoomType
            {
                RoomTypeId = model.RoomTypeId,
                HotelId = model.HotelId,
                Name = model.Name,
                Description = model.Description,
                Capacity = model.Capacity,
                BasePrice = model.BasePrice,
                ImageUrl = model.ImageUrl
            };

            await _roomService.UpdateRoomTypeAsync(roomType);
            TempData["Success"] = "Room Type updated successfully!";
            return RedirectToAction("RoomTypes");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /HotelOwner/Rooms
    [HttpGet]
    public async Task<IActionResult> Rooms(int? floor = null, string? status = null)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        var rooms = await _roomService.GetHotelRoomsAsync(hotelId.Value);
        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId.Value);

        // Filter
        if (floor.HasValue)
        {
            rooms = rooms.Where(r => r.Floor == floor.Value);
        }
        if (!string.IsNullOrEmpty(status))
        {
            rooms = rooms.Where(r => r.Status == status);
        }

        // Get RoomTypes for dropdowns/modals
        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(hotelId.Value);
        ViewBag.RoomTypes = roomTypes;

        var model = new AdminRoomListViewModel
        {
            HotelId = hotelId.Value,
            HotelName = hotel?.Name ?? "Unknown Hotel",
            StatusFilter = status,
            FloorFilter = floor,
            Rooms = rooms.OrderBy(r => r.Floor).ThenBy(r => r.RoomNumber).Select(r => new AdminRoomItemViewModel
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                RoomTypeName = r.RoomType.Name,
                Floor = r.Floor,
                Status = r.Status,
                BasePrice = r.RoomType.BasePrice,
                Capacity = r.RoomType.Capacity
            })
        };

        return View(model);
    }

    // POST: /HotelOwner/CreateRooms
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRooms(RoomBulkCreateViewModel model)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue || hotelId.Value != model.HotelId) return Forbid();

        if (!ModelState.IsValid) return RedirectToAction("Rooms");

        try
        {
            await _roomService.AddRoomsBulkAsync(
                model.RoomTypeId, 
                model.StartingNumber, 
                model.Quantity, 
                model.Floor);
            
            TempData["Success"] = $"{model.Quantity} rooms created successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Rooms");
    }

    // POST: /HotelOwner/DeleteRoom/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return Forbid();

        // Verify ownership implicitly via service or room check
        // Ideally GetRoomDetails should be checked first
        var room = await _roomService.GetRoomDetailsAsync(id);
        if (room == null) return NotFound();
        if (room.RoomType.HotelId != hotelId.Value) return Forbid();

        try
        {
            await _roomService.DeleteRoomAsync(id);
            TempData["Success"] = "Room deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Rooms");
    }

    // GET: /HotelOwner/Bookings
    [HttpGet]
    public async Task<IActionResult> Bookings(string? status = null)
    {
        var hotelId = await GetUserHotelIdAsync();
        if (!hotelId.HasValue) return RedirectToAction("Dashboard");

        var bookings = await _bookingService.GetHotelBookingsAsync(hotelId.Value);
        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId.Value);

        if (!string.IsNullOrEmpty(status))
        {
            bookings = bookings.Where(b => b.Status == status);
        }

        var model = new AdminBookingListViewModel
        {
            HotelId = hotelId.Value,
            HotelName = hotel?.Name,
            StatusFilter = status,
            Bookings = bookings.Select(b => new AdminBookingItemViewModel
            {
                BookingId = b.BookingId,
                GuestName = b.User.FullName,
                GuestEmail = b.User.Email,
                GuestPhone = b.User.PhoneNumber ?? "N/A",
                HotelName = b.Room.RoomType.Hotel.Name,
                RoomNumber = b.Room.RoomNumber,
                RoomTypeName = b.Room.RoomType.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                GuestCount = b.GuestCount,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                SpecialRequests = b.SpecialRequests,
                CreatedDate = b.CreatedDate
            })
        };

        return View(model);
    }

    // POST: /HotelOwner/ConfirmBooking/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmBooking(int id)
    {
        try
        {
            await _bookingService.ConfirmBookingAsync(id);
            TempData["Success"] = "Booking confirmed.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to confirm booking. Please try again.";
        }
        return RedirectToAction("Bookings");
    }

    // POST: /HotelOwner/CheckIn/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int id)
    {
        try
        {
            await _bookingService.CheckInAsync(id);
            TempData["Success"] = "Guest checked in.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to check in guest. Please verify booking status.";
        }
        return RedirectToAction("Bookings");
    }

    // POST: /HotelOwner/CheckOut/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int id)
    {
        try
        {
            await _bookingService.CheckOutAsync(id);
            TempData["Success"] = "Guest checked out.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to check out guest. Please verify booking status.";
        }
        return RedirectToAction("Bookings");
    }

    // POST: /HotelOwner/CancelBooking/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int id)
    {
        try
        {
            await _bookingService.CancelBookingAsync(id);
            TempData["Success"] = "Booking cancelled.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Unable to cancel booking. Please try again.";
        }
        return RedirectToAction("Bookings");
    }
}
