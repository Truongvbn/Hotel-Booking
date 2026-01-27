using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelBooking.ViewModels;
using Services.Interfaces;
using DataAccess.Models;

namespace HotelBooking.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IHotelService _hotelService;
    private readonly IRoomService _roomService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public AdminController(
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



    // GET: /Admin/Dashboard
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        IEnumerable<Hotel> hotels;
        
        hotels = await _hotelService.GetAllHotelsAsync();

        var totalRooms = 0;
    var availableRooms = 0;
    var occupiedRooms = 0;
    var totalBookings = 0;
    var pendingBookings = 0;
    var todayCheckIns = 0;
    var todayCheckOuts = 0;
    decimal totalRevenue = 0;
    var recentBookings = new List<Booking>();

    foreach (var hotel in hotels)
    {
        var rooms = await _roomService.GetHotelRoomsAsync(hotel.HotelId);
        totalRooms += rooms.Count();
        availableRooms += rooms.Count(r => r.Status == "Available");
        occupiedRooms += rooms.Count(r => r.Status == "Occupied");

        var bookings = await _bookingService.GetHotelBookingsAsync(hotel.HotelId);
        totalBookings += bookings.Count();
        pendingBookings += bookings.Count(b => b.Status == "Pending");
        todayCheckIns += bookings.Count(b => b.CheckInDate.Date == DateTime.Today && b.Status == "Confirmed");
        todayCheckOuts += bookings.Count(b => b.CheckOutDate.Date == DateTime.Today && b.Status == "CheckedIn");
        totalRevenue += bookings.Where(b => b.Status != "Cancelled").Sum(b => b.TotalPrice);
        
        recentBookings.AddRange(bookings);
    }

    var model = new DashboardViewModel
    {
        TotalHotels = hotels.Count(),
        TotalRooms = totalRooms,
        AvailableRooms = availableRooms,
        OccupiedRooms = occupiedRooms,
        TotalBookings = totalBookings,
        PendingBookings = pendingBookings,
        TodayCheckIns = todayCheckIns,
        TodayCheckOuts = todayCheckOuts,
        TotalRevenue = totalRevenue,
        OccupancyRate = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0,
        RecentBookings = recentBookings
            .OrderByDescending(b => b.CreatedDate)
            .Take(5)
            .Select(b => new BookingItemViewModel
            {
                BookingId = b.BookingId,
                HotelName = b.Room.RoomType.Hotel.Name,
                RoomTypeName = b.Room.RoomType.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status
            })
    };

    return View(model);
}

    #region Hotels Management

    // GET: /Admin/Hotels
    [HttpGet]
    public async Task<IActionResult> Hotels()
    {
        IEnumerable<Hotel> hotels;
        
        hotels = await _hotelService.GetAllHotelsAsync();
        
        var model = new AdminHotelListViewModel
        {
            Hotels = hotels.Select(h => new AdminHotelItemViewModel
            {
                HotelId = h.HotelId,
                Name = h.Name,
                City = h.City,
                StarRating = h.StarRating,
                TotalRooms = h.RoomTypes.SelectMany(rt => rt.Rooms).Count(),
                AvailableRooms = h.RoomTypes.SelectMany(rt => rt.Rooms).Count(r => r.Status == "Available"),
                IsActive = h.IsActive,
                CreatedDate = h.CreatedDate
            })
        };


        return View(model);
    }

    // GET: /Admin/CreateHotel
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateHotel()
    {
        return View(new HotelFormViewModel());
    }

    // POST: /Admin/CreateHotel
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateHotel(HotelFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var hotel = new Hotel
            {
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

            await _hotelService.AddHotelAsync(hotel);
            TempData["Success"] = "Hotel created successfully!";
            return RedirectToAction("Hotels");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /Admin/EditHotel/5
    [HttpGet]
    public async Task<IActionResult> EditHotel(int id)
    {


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

    // POST: /Admin/EditHotel
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHotel(HotelFormViewModel model)
    {


        if (!ModelState.IsValid)
        {

            return View(model);
        }

        try
        {
            var hotel = new Hotel
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
            return RedirectToAction("Hotels");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(model);
        }
    }

    // POST: /Admin/DeleteHotel/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        try
        {
            await _hotelService.DeleteHotelAsync(id);
            TempData["Success"] = "Hotel deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Hotels");
    }

    #endregion

    #region Rooms Management

    // GET: /Admin/Rooms/5
    [HttpGet]
    public async Task<IActionResult> Rooms(int hotelId, string? status = null)
    {


        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        var rooms = await _roomService.GetHotelRoomsAsync(hotelId);
        if (!string.IsNullOrEmpty(status))
        {
            rooms = rooms.Where(r => r.Status == status);
        }

        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(hotelId);

        var model = new AdminRoomListViewModel
        {
            HotelId = hotelId,
            HotelName = hotel.Name,
            StatusFilter = status,
            Rooms = rooms.Select(r => new AdminRoomItemViewModel
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

        ViewBag.RoomTypes = roomTypes;

        return View(model);
    }

    // POST: /Admin/UpdateRoomStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRoomStatus(int roomId, string status, int hotelId)
    {
        try
        {
            await _roomService.UpdateRoomStatusAsync(roomId, status);
            TempData["Success"] = "Room status updated!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Rooms", new { hotelId });
    }

    // POST: /Admin/CreateRooms - Bulk create rooms
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRooms(RoomBulkCreateViewModel model)
    {
        try
        {
            await _roomService.AddRoomsBulkAsync(model.RoomTypeId, model.StartingNumber, model.Quantity, model.Floor);
            TempData["Success"] = $"Successfully created {model.Quantity} room(s)!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Rooms", new { hotelId = model.HotelId });
    }

    // POST: /Admin/DeleteRoom
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRoom(int roomId, int hotelId)
    {
        try
        {
            await _roomService.DeleteRoomAsync(roomId);
            TempData["Success"] = "Room deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Rooms", new { hotelId });
    }

    #endregion

    #region RoomTypes Management

    // GET: /Admin/RoomTypes/5
    [HttpGet]
    public async Task<IActionResult> RoomTypes(int hotelId)
    {


        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(hotelId);

        ViewBag.HotelId = hotelId;
        ViewBag.HotelName = hotel.Name;

        return View(roomTypes);
    }

    // GET: /Admin/CreateRoomType
    [HttpGet]
    public async Task<IActionResult> CreateRoomType(int hotelId)
    {
        var hotel = await _hotelService.GetHotelDetailsAsync(hotelId);
        if (hotel == null) return NotFound();

        var model = new RoomTypeFormViewModel
        {
            HotelId = hotelId
        };

        ViewBag.HotelName = hotel.Name;
        return View(model);
    }

    // POST: /Admin/CreateRoomType
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoomType(RoomTypeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var hotel = await _hotelService.GetHotelDetailsAsync(model.HotelId);
            ViewBag.HotelName = hotel?.Name ?? "";
            return View(model);
        }

        try
        {
            await _roomService.AddRoomTypeAsync(model.HotelId, model.Name, model.Description, model.Capacity, model.BasePrice);
            TempData["Success"] = "Room type created successfully!";
            return RedirectToAction("RoomTypes", new { hotelId = model.HotelId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var hotel = await _hotelService.GetHotelDetailsAsync(model.HotelId);
            ViewBag.HotelName = hotel?.Name ?? "";
            return View(model);
        }
    }

    // GET: /Admin/EditRoomType/5
    [HttpGet]
    public async Task<IActionResult> EditRoomType(int id)
    {
        var roomTypes = await _roomService.GetRoomTypesByHotelAsync(0); // We need to get by ID
        // For now, get all room types and filter - should optimize later
        var hotels = await _hotelService.GetAllHotelsAsync();
        RoomType? roomType = null;
        Hotel? hotel = null;

        foreach (var h in hotels)
        {
            var types = await _roomService.GetRoomTypesByHotelAsync(h.HotelId);
            roomType = types.FirstOrDefault(rt => rt.RoomTypeId == id);
            if (roomType != null)
            {
                hotel = h;
                break;
            }
        }

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

        ViewBag.HotelName = hotel?.Name ?? "";
        return View(model);
    }

    // POST: /Admin/EditRoomType
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoomType(RoomTypeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var hotel = await _hotelService.GetHotelDetailsAsync(model.HotelId);
            ViewBag.HotelName = hotel?.Name ?? "";
            return View(model);
        }

        try
        {
            var roomType = new RoomType
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
            TempData["Success"] = "Room type updated successfully!";
            return RedirectToAction("RoomTypes", new { hotelId = model.HotelId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var hotel = await _hotelService.GetHotelDetailsAsync(model.HotelId);
            ViewBag.HotelName = hotel?.Name ?? "";
            return View(model);
        }
    }

    #endregion

    #region Bookings Management

    // GET: /Admin/Bookings
    [HttpGet]
    public async Task<IActionResult> Bookings(int? hotelId = null, string? status = null)
    {
        IEnumerable<Booking> bookings;
        string? hotelName = null;



        if (hotelId.HasValue)
        {
            var hotel = await _hotelService.GetHotelDetailsAsync(hotelId.Value);
            hotelName = hotel?.Name;
            bookings = await _bookingService.GetHotelBookingsAsync(hotelId.Value);
        }
        else
        {
            // Admin only: Get all bookings from all hotels
            var hotels = await _hotelService.GetAllHotelsAsync();
            var allBookings = new List<Booking>();
            foreach (var hotel in hotels)
            {
                var hotelBookings = await _bookingService.GetHotelBookingsAsync(hotel.HotelId);
                allBookings.AddRange(hotelBookings);
            }
            bookings = allBookings.OrderByDescending(b => b.CreatedDate);
        }

        if (!string.IsNullOrEmpty(status))
        {
            bookings = bookings.Where(b => b.Status == status);
        }

        var model = new AdminBookingListViewModel
        {
            HotelId = hotelId,
            HotelName = hotelName,
            StatusFilter = status,
            Bookings = bookings.Select(b => new AdminBookingItemViewModel
            {
                BookingId = b.BookingId,
                GuestName = b.User?.FullName ?? "Unknown",
                GuestEmail = b.User?.Email ?? "",
                GuestPhone = b.User?.PhoneNumber ?? "",
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

    // POST: /Admin/ConfirmBooking/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmBooking(int id)
    {
        try
        {
            await _bookingService.ConfirmBookingAsync(id);
            TempData["Success"] = "Booking confirmed!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Bookings");
    }

    // POST: /Admin/CheckIn/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int id)
    {
        try
        {
            await _bookingService.CheckInAsync(id);
            TempData["Success"] = "Guest checked in successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Bookings");
    }

    // POST: /Admin/CheckOut/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int id)
    {
        try
        {
            await _bookingService.CheckOutAsync(id);
            TempData["Success"] = "Guest checked out successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Bookings");
    }

    // POST: /Admin/CancelBooking/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int id)
    {
        try
        {
            await _bookingService.CancelBookingAsync(id);
            TempData["Success"] = "Booking cancelled!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Bookings");
    }

    #endregion
}
