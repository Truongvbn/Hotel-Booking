using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId);
    Task<IEnumerable<Room>> GetRoomsByStatusAsync(int hotelId, string status);
    Task<Room?> GetRoomWithDetailsAsync(int roomId);
    Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId);
    Task UpdateRoomStatusAsync(int roomId, string status);
}
