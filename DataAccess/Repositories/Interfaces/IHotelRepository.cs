using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<IEnumerable<Hotel>> GetHotelsWithRoomTypesAsync();
    Task<IEnumerable<Hotel>> SearchByCityAsync(string city);
    Task<Hotel?> GetHotelWithDetailsAsync(int hotelId);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut, int capacity);
    Task<decimal> GetAverageRatingAsync(int hotelId);
    Task<IEnumerable<string>> GetAllCitiesAsync();
}
