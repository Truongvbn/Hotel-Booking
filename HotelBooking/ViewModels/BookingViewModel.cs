using System.ComponentModel.DataAnnotations;
using DataAccess.Models;
using HotelBooking.Validation;

namespace HotelBooking.ViewModels;

public class BookingViewModel
{
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Check-in date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Check-in Date")]
    [FutureDate(ErrorMessage = "Check-in date cannot be in the past")]
    public DateTime CheckInDate { get; set; }

    [Required(ErrorMessage = "Check-out date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Check-out Date")]
    [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be after check-in date")]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 10, ErrorMessage = "Guest count must be between 1 and 10")]
    [Display(Name = "Number of Guests")]
    public int GuestCount { get; set; } = 1;

    [Display(Name = "Special Requests")]
    [StringLength(500)]
    public string? SpecialRequests { get; set; }

    // Display properties
    public string HotelName { get; set; } = string.Empty;
    public string RoomTypeName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public int Nights { get; set; }
    public decimal TotalPrice { get; set; }
    public string? HotelImageUrl { get; set; }
}

public class BookingConfirmationViewModel
{
    public int BookingId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string RoomTypeName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class MyBookingsViewModel
{
    public IEnumerable<BookingItemViewModel> Bookings { get; set; } = new List<BookingItemViewModel>();
}

public class BookingItemViewModel
{
    public int BookingId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string RoomTypeName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int GuestCount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? HotelImageUrl { get; set; }
    
    public string StatusBadgeClass => Status switch
    {
        "Pending" => "badge-warning",
        "Confirmed" => "badge-info",
        "CheckedIn" => "badge-primary",
        "CheckedOut" => "badge-success",
        "Cancelled" => "badge-error",
        _ => "badge-ghost"
    };
}

public class RecalculatePriceRequest
{
    public int RoomId { get; set; }
    public string CheckInDate { get; set; } = string.Empty;
    public string CheckOutDate { get; set; } = string.Empty;
}

