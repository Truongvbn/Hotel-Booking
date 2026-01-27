using DataAccess.Models;

namespace HotelBooking.ViewModels;

public class HotelSearchResultViewModel
{
    public SearchViewModel SearchCriteria { get; set; } = new();
    public IEnumerable<HotelItemViewModel> Hotels { get; set; } = new List<HotelItemViewModel>();
    public IEnumerable<string> AvailableCities { get; set; } = new List<string>();
}

public class HotelItemViewModel
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public decimal StarRating { get; set; }
    public string? ImageUrl { get; set; }
    public decimal MinPrice { get; set; }
    public int AvailableRoomsCount { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

public class HotelDetailsViewModel
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
    
    // Search criteria
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int Guests { get; set; }
    public int Nights { get; set; }
    
    // Available rooms
    public IEnumerable<RoomTypeViewModel> RoomTypes { get; set; } = new List<RoomTypeViewModel>();
    
    // Reviews
    public decimal AverageRating { get; set; }
    public IEnumerable<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
}

public class RoomTypeViewModel
{
    public int RoomTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public int AvailableCount { get; set; }
    public IEnumerable<RoomViewModel> AvailableRooms { get; set; } = new List<RoomViewModel>();
    
    // Calculated for display
    public decimal TotalPrice { get; set; }
    public int Nights { get; set; }
}

public class RoomViewModel
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Status { get; set; } = string.Empty;
    public IEnumerable<string> Amenities { get; set; } = new List<string>();
}

public class ReviewViewModel
{
    public int ReviewId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedDate { get; set; }
}
