using DataAccess.Models;

namespace Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId);
    Task<Room?> GetRoomDetailsAsync(int roomId);
    Task<IEnumerable<Room>> GetRoomsByStatusAsync(int hotelId, string status);
    Task UpdateRoomStatusAsync(int roomId, string status);
    
    // Room Type operations
    Task<IEnumerable<RoomType>> GetRoomTypesByHotelAsync(int hotelId);
    Task<RoomType> AddRoomTypeAsync(int hotelId, string name, string? description, int capacity, decimal basePrice);
    Task<RoomType> UpdateRoomTypeAsync(RoomType roomType);
    
    // Room operations
    Task<Room> AddRoomAsync(int userId, int roomTypeId, string roomNumber, int floor, string? status = null);
    Task AddRoomsBulkAsync(int roomTypeId, int startingNumber, int quantity, int floor);
    Task<Room> UpdateRoomAsync(int roomId, string roomNumber, int floor, string status);
    Task DeleteRoomAsync(int roomId);
}
