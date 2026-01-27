using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<Room>> GetHotelRoomsAsync(int hotelId)
    {
        return await _roomRepository.GetHotelRoomsAsync(hotelId);
    }

    public async Task<Room?> GetRoomDetailsAsync(int roomId)
    {
        return await _roomRepository.GetRoomWithDetailsAsync(roomId);
    }

    public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(int hotelId, string status)
    {
        return await _roomRepository.GetRoomsByStatusAsync(hotelId, status);
    }

    public async Task UpdateRoomStatusAsync(int roomId, string status)
    {
        var validStatuses = new[] { "Available", "Occupied", "Maintenance" };
        if (!validStatuses.Contains(status))
        {
            throw new ArgumentException($"Invalid status. Valid values: {string.Join(", ", validStatuses)}");
        }

        await _roomRepository.UpdateRoomStatusAsync(roomId, status);
    }

    // Room Type operations
    public async Task<IEnumerable<RoomType>> GetRoomTypesByHotelAsync(int hotelId)
    {
        return await _roomTypeRepository.GetByHotelIdAsync(hotelId);
    }

    public async Task<RoomType> AddRoomTypeAsync(int hotelId, string name, string? description, int capacity, decimal basePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Room type name is required");
        }

        if (capacity < 1)
        {
            throw new ArgumentException("Capacity must be at least 1");
        }

        if (basePrice < 0)
        {
            throw new ArgumentException("Base price cannot be negative");
        }

        var roomType = new RoomType
        {
            HotelId = hotelId,
            Name = name,
            Description = description,
            Capacity = capacity,
            BasePrice = basePrice,
            CreatedDate = DateTime.Now
        };

        return await _roomTypeRepository.AddAsync(roomType);
    }

    public async Task<RoomType> UpdateRoomTypeAsync(RoomType roomType)
    {
        var existing = await _roomTypeRepository.GetByIdAsync(roomType.RoomTypeId);
        if (existing == null)
        {
            throw new ArgumentException("Room type not found");
        }

        existing.Name = roomType.Name;
        existing.Description = roomType.Description;
        existing.Capacity = roomType.Capacity;
        existing.BasePrice = roomType.BasePrice;
        existing.ImageUrl = roomType.ImageUrl;
        existing.UpdatedDate = DateTime.Now;

        await _roomTypeRepository.UpdateAsync(existing);
        return existing;
    }

    // Room operations
    public async Task AddRoomsBulkAsync(int roomTypeId, int startingNumber, int quantity, int floor)
    {
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity must be at least 1");
        }

        var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId);
        if (roomType == null)
        {
            throw new ArgumentException("Room type not found");
        }

        for (int i = 0; i < quantity; i++)
        {
            var room = new Room
            {
                RoomTypeId = roomTypeId,
                RoomNumber = (startingNumber + i).ToString(),
                Floor = floor,
                Status = "Available",
                CreatedDate = DateTime.Now
            };
            await _roomRepository.AddAsync(room);
        }
    }

    public async Task<Room> UpdateRoomAsync(int roomId, string roomNumber, int floor, string status)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        room.RoomNumber = roomNumber;
        room.Floor = floor;
        room.Status = status;
        room.UpdatedDate = DateTime.Now;

        await _roomRepository.UpdateAsync(room);
        return room;
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        await _roomRepository.DeleteAsync(roomId);
    }
}
