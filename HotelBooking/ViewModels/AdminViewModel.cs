using DataAccess.Models;

namespace HotelBooking.ViewModels;

public class DashboardViewModel
{
    public int TotalHotels { get; set; }
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int TodayCheckIns { get; set; }
    public int TodayCheckOuts { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal OccupancyRate { get; set; }
    
    public IEnumerable<BookingItemViewModel> RecentBookings { get; set; } = new List<BookingItemViewModel>();
}

public class AdminHotelListViewModel
{
    public IEnumerable<AdminHotelItemViewModel> Hotels { get; set; } = new List<AdminHotelItemViewModel>();
}

public class AdminHotelItemViewModel
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public decimal StarRating { get; set; }
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class AdminRoomListViewModel
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public IEnumerable<AdminRoomItemViewModel> Rooms { get; set; } = new List<AdminRoomItemViewModel>();
    
    // Filter options
    public string? StatusFilter { get; set; }
    public int? FloorFilter { get; set; }
}

public class AdminRoomItemViewModel
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomTypeName { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int Capacity { get; set; }
    
    public string StatusBadgeClass => Status switch
    {
        "Available" => "badge-success",
        "Occupied" => "badge-error",
        "Maintenance" => "badge-warning",
        _ => "badge-ghost"
    };
}

public class AdminBookingListViewModel
{
    public int? HotelId { get; set; }
    public string? HotelName { get; set; }
    public IEnumerable<AdminBookingItemViewModel> Bookings { get; set; } = new List<AdminBookingItemViewModel>();
    
    // Filter options
    public string? StatusFilter { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class AdminBookingItemViewModel
{
    public int BookingId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomTypeName { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public string StatusBadgeClass => Status switch
    {
        "Pending" => "badge-warning",
        "Confirmed" => "badge-info",
        "CheckedIn" => "badge-primary",
        "CheckedOut" => "badge-success",
        "Cancelled" => "badge-error",
        _ => "badge-ghost"
    };
    
    public bool CanConfirm => Status == "Pending";
    public bool CanCheckIn => Status == "Confirmed";
    public bool CanCheckOut => Status == "CheckedIn";
    public bool CanCancel => Status == "Pending" || Status == "Confirmed";
}

public class HotelFormViewModel
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public decimal StarRating { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

public class RoomTypeFormViewModel
{
    public int RoomTypeId { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; } = 2;
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
}

public class RoomBulkCreateViewModel
{
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public int StartingNumber { get; set; } = 101;
    public int Quantity { get; set; } = 1;
    public int Floor { get; set; } = 1;
}
