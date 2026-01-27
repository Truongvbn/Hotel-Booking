using Microsoft.EntityFrameworkCore;
using DataAccess.Context;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories;

public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(HotelBookingContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId)
    {
        return await _dbSet
            .Include(rt => rt.Rooms)
            .Where(rt => rt.HotelId == hotelId)
            .OrderBy(rt => rt.BasePrice)
            .ToListAsync();
    }

    public async Task<RoomType?> GetWithRoomsAsync(int roomTypeId)
    {
        return await _dbSet
            .Include(rt => rt.Hotel)
            .Include(rt => rt.Rooms)
            .FirstOrDefaultAsync(rt => rt.RoomTypeId == roomTypeId);
    }
}
