using DataAccess.Models;

namespace Services.Interfaces;

public interface IHotelService
{
    Task<IEnumerable<Hotel>> GetAllHotelsAsync();
    Task<Hotel?> GetHotelDetailsAsync(int hotelId);
    Task<IEnumerable<Hotel>> SearchByCityAsync(string city);
    Task<IEnumerable<Room>> SearchAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int capacity);
    Task<decimal> GetHotelRatingAsync(int hotelId);
    Task<IEnumerable<string>> GetAllCitiesAsync();
    
    // Admin operations
    Task<Hotel> AddHotelAsync(Hotel hotel);
    Task<Hotel> UpdateHotelAsync(Hotel hotel);
    Task DeleteHotelAsync(int hotelId);
}
