using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(HotelBookingContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId)
    {
        return await _dbSet
            .Include(r => r.RoomType)
            .Include(r => r.Bookings)
            .Where(r => r.RoomType.HotelId == hotelId)
            .OrderBy(r => r.Floor)
            .ThenBy(r => r.RoomNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(int hotelId, string status)
    {
        return await _dbSet
            .Include(r => r.RoomType)
            .Where(r => r.RoomType.HotelId == hotelId && r.Status == status)
            .ToListAsync();
    }

    public async Task<Room?> GetRoomWithDetailsAsync(int roomId)
    {
        return await _dbSet
            .Include(r => r.RoomType)
                .ThenInclude(rt => rt.Hotel)
            .Include(r => r.Bookings)
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenity)
            .FirstOrDefaultAsync(r => r.RoomId == roomId);
    }

    public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId)
    {
        return await _dbSet
            .Include(r => r.RoomType)
            .Where(r => r.RoomTypeId == roomTypeId)
            .ToListAsync();
    }

    public async Task UpdateRoomStatusAsync(int roomId, string status)
    {
        var room = await GetByIdAsync(roomId);
        if (room != null)
        {
            room.Status = status;
            room.UpdatedDate = DateTime.Now;
            await UpdateAsync(room);
        }
    }
}
