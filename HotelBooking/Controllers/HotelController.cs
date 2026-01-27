using Microsoft.AspNetCore.Mvc;
using HotelBooking.ViewModels;
using Services.Interfaces;

namespace HotelBooking.Controllers;

public class HotelController : Controller
{
    private readonly IHotelService _hotelService;

    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    // GET: /Hotel/Search
    [HttpGet]
    public async Task<IActionResult> Search(SearchViewModel model)
    {
        var cities = await _hotelService.GetAllCitiesAsync();
        
        // If no search criteria, show all hotels
        IEnumerable<DataAccess.Models.Hotel> hotels;
        if (string.IsNullOrWhiteSpace(model.City))
        {
            hotels = await _hotelService.GetAllHotelsAsync();
        }
        else
        {
            hotels = await _hotelService.SearchByCityAsync(model.City);
        }

        var hotelViewModels = new List<HotelItemViewModel>();
        foreach (var hotel in hotels)
        {
            var rating = await _hotelService.GetHotelRatingAsync(hotel.HotelId);
            var availableRooms = await _hotelService.SearchAvailableRoomsAsync(
                hotel.HotelId, 
                model.CheckInDate, 
                model.CheckOutDate, 
                model.Guests);

            var minPrice = hotel.RoomTypes.Any() 
                ? hotel.RoomTypes.Min(rt => rt.BasePrice) 
                : 0;

            hotelViewModels.Add(new HotelItemViewModel
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Description = hotel.Description,
                StarRating = hotel.StarRating,
                ImageUrl = hotel.ImageUrl,
                MinPrice = minPrice,
                AvailableRoomsCount = availableRooms.Count(),
                AverageRating = rating,
                ReviewCount = hotel.Reviews?.Count() ?? 0
            });
        }

        var result = new HotelSearchResultViewModel
        {
            SearchCriteria = model,
            Hotels = hotelViewModels.Where(h => h.AvailableRoomsCount > 0),
            AvailableCities = cities
        };

        return View(result);
    }

    // GET: /Hotel/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id, DateTime? checkIn, DateTime? checkOut, int? guests)
    {
        var checkInDate = checkIn ?? DateTime.Today;
        var checkOutDate = checkOut ?? DateTime.Today.AddDays(1);
        var guestCount = guests ?? 2;

        var hotel = await _hotelService.GetHotelDetailsAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }

        var availableRooms = await _hotelService.SearchAvailableRoomsAsync(id, checkInDate, checkOutDate, guestCount);
        var rating = await _hotelService.GetHotelRatingAsync(id);
        var nights = (int)(checkOutDate - checkInDate).TotalDays;

        // Group rooms by room type
        var roomTypeViewModels = new List<RoomTypeViewModel>();
        foreach (var roomType in hotel.RoomTypes)
        {
            var roomsOfType = availableRooms.Where(r => r.RoomTypeId == roomType.RoomTypeId).ToList();
            if (roomsOfType.Any())
            {
                roomTypeViewModels.Add(new RoomTypeViewModel
                {
                    RoomTypeId = roomType.RoomTypeId,
                    Name = roomType.Name,
                    Description = roomType.Description,
                    Capacity = roomType.Capacity,
                    BasePrice = roomType.BasePrice,
                    ImageUrl = roomType.ImageUrl,
                    AvailableCount = roomsOfType.Count,
                    Nights = nights,
                    TotalPrice = roomType.BasePrice * nights,
                    AvailableRooms = roomsOfType.Select(r => new RoomViewModel
                    {
                        RoomId = r.RoomId,
                        RoomNumber = r.RoomNumber,
                        Floor = r.Floor,
                        Status = r.Status,
                        Amenities = r.RoomAmenities?.Select(ra => ra.Amenity.Name) ?? new List<string>()
                    })
                });
            }
        }

        var reviewViewModels = hotel.Reviews?
            .Where(r => r.IsApproved)
            .OrderByDescending(r => r.CreatedDate)
            .Take(10)
            .Select(r => new ReviewViewModel
            {
                ReviewId = r.ReviewId,
                UserName = r.User?.FullName ?? "Anonymous",
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate
            }) ?? new List<ReviewViewModel>();

        var model = new HotelDetailsViewModel
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
            Longitude = hotel.Longitude,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            Guests = guestCount,
            Nights = nights,
            RoomTypes = roomTypeViewModels,
            AverageRating = rating,
            Reviews = reviewViewModels
        };

        return View(model);
    }
}
