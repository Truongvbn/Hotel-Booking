using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

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
    
    [Required(ErrorMessage = "Hotel name is required")]
    [StringLength(200, ErrorMessage = "Hotel name cannot exceed 200 characters")]
    [Display(Name = "Hotel Name")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    [Display(Name = "Address")]
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }
    
    [StringLength(100)]
    [Display(Name = "Country")]
    public string? Country { get; set; }
    
    [StringLength(2000)]
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5")]
    [Display(Name = "Star Rating")]
    public decimal StarRating { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }
    
    [Url(ErrorMessage = "Invalid website URL")]
    [Display(Name = "Website")]
    public string? Website { get; set; }
    
    [Url(ErrorMessage = "Invalid image URL")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }
    
    [Display(Name = "Latitude")]
    public decimal? Latitude { get; set; }
    
    [Display(Name = "Longitude")]
    public decimal? Longitude { get; set; }
}

public class RoomTypeFormViewModel
{
    public int RoomTypeId { get; set; }
    public int HotelId { get; set; }
    
    [Required(ErrorMessage = "Room type name is required")]
    [StringLength(100)]
    [Display(Name = "Room Type Name")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000)]
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
    [Display(Name = "Capacity")]
    public int Capacity { get; set; } = 2;
    
    [Required(ErrorMessage = "Base price is required")]
    [Range(0.01, 100000000, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Base Price")]
    public decimal BasePrice { get; set; }
    
    [Url(ErrorMessage = "Invalid image URL")]
    [Display(Name = "Image URL")]
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

public class AdminRoomTypeListViewModel
{
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public IEnumerable<AdminRoomTypeItemViewModel> RoomTypes { get; set; } = new List<AdminRoomTypeItemViewModel>();
}

public class AdminRoomTypeItemViewModel
{
    public int RoomTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public int RoomCount { get; set; }
}
