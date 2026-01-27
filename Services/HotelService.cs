using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
    {
        return await _hotelRepository.GetHotelsWithRoomTypesAsync();
    }

    public async Task<Hotel?> GetHotelDetailsAsync(int hotelId)
    {
        return await _hotelRepository.GetHotelWithDetailsAsync(hotelId);
    }

    public async Task<IEnumerable<Hotel>> SearchByCityAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return await GetAllHotelsAsync();
        }
        return await _hotelRepository.SearchByCityAsync(city);
    }

    public async Task<IEnumerable<Room>> SearchAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int capacity)
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

        if (capacity < 1)
        {
            throw new ArgumentException("Capacity must be at least 1");
        }

        return await _hotelRepository.GetAvailableRoomsAsync(hotelId, checkIn, checkOut, capacity);
    }

    public async Task<decimal> GetHotelRatingAsync(int hotelId)
    {
        return await _hotelRepository.GetAverageRatingAsync(hotelId);
    }

    public async Task<IEnumerable<string>> GetAllCitiesAsync()
    {
        return await _hotelRepository.GetAllCitiesAsync();
    }

    // Admin operations
    public async Task<Hotel> AddHotelAsync(Hotel hotel)
    {
        if (string.IsNullOrWhiteSpace(hotel.Name))
        {
            throw new ArgumentException("Hotel name is required");
        }

        hotel.CreatedDate = DateTime.UtcNow;
        hotel.IsActive = true;

        return await _hotelRepository.AddAsync(hotel);
    }

    public async Task<Hotel> UpdateHotelAsync(Hotel hotel)
    {
        var existingHotel = await _hotelRepository.GetByIdAsync(hotel.HotelId);
        if (existingHotel == null)
        {
            throw new ArgumentException("Hotel not found");
        }

        existingHotel.Name = hotel.Name;
        existingHotel.Address = hotel.Address;
        existingHotel.City = hotel.City;
        existingHotel.Country = hotel.Country;
        existingHotel.Description = hotel.Description;
        existingHotel.StarRating = hotel.StarRating;
        existingHotel.Phone = hotel.Phone;
        existingHotel.Website = hotel.Website;
        existingHotel.ImageUrl = hotel.ImageUrl;
        existingHotel.Latitude = hotel.Latitude;
        existingHotel.Longitude = hotel.Longitude;
        existingHotel.UpdatedDate = DateTime.UtcNow;

        await _hotelRepository.UpdateAsync(existingHotel);
        return existingHotel;
    }

    public async Task DeleteHotelAsync(int hotelId)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);
        if (hotel == null)
        {
            throw new ArgumentException("Hotel not found");
        }

        // Soft delete
        hotel.IsActive = false;
        hotel.UpdatedDate = DateTime.UtcNow;
        await _hotelRepository.UpdateAsync(hotel);
    }
}
