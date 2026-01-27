using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IRoomTypeRepository : IRepository<RoomType>
{
    Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId);
    Task<RoomType?> GetWithRoomsAsync(int roomTypeId);
}
