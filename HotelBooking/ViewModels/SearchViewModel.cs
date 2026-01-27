using System.ComponentModel.DataAnnotations;
using HotelBooking.Validation;

namespace HotelBooking.ViewModels;

public class SearchViewModel
{
    [Display(Name = "City")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Check-in date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Check-in Date")]
    [FutureDate(ErrorMessage = "Check-in date cannot be in the past")]
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Check-out date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Check-out Date")]
    [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be after check-in date")]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
    [Display(Name = "Guests")]
    public int Guests { get; set; } = 2;
}
