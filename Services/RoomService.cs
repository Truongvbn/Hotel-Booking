using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IUserRepository _userRepository;

    public RoomService(IRoomRepository roomRepository, IRoomTypeRepository roomTypeRepository, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _userRepository = userRepository;
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
            CreatedDate = DateTime.UtcNow
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
        existing.UpdatedDate = DateTime.UtcNow;

        await _roomTypeRepository.UpdateAsync(existing);
        return existing;
    }

    // Room operations
    public async Task<Room> AddRoomAsync(int userId, int roomTypeId, string roomNumber, int floor, string? status = null)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            throw new ArgumentException("Room number is required");
        }

        if (floor < 0)
        {
            throw new ArgumentException("Floor must be a non-negative number");
        }

        // Verify user exists and is a hotel owner
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        if (user.Role != "HotelOwner")
        {
            throw new UnauthorizedAccessException("Only hotel owners can add rooms");
        }

        if (!user.HotelId.HasValue)
        {
            throw new ArgumentException("User is not associated with a hotel");
        }

        // Verify room type exists and belongs to the user's hotel
        var roomType = await _roomTypeRepository.GetWithRoomsAsync(roomTypeId);
        if (roomType == null)
        {
            throw new ArgumentException("Room type not found");
        }

        if (roomType.HotelId != user.HotelId.Value)
        {
            throw new UnauthorizedAccessException("Room type does not belong to your hotel");
        }

        // Check if room number already exists for this room type
        if (await _roomRepository.IsRoomNumberExistsAsync(roomTypeId, roomNumber))
        {
            throw new ArgumentException($"Room number '{roomNumber}' already exists for this room type");
        }

        // Validate status if provided
        var validStatuses = new[] { "Available", "Occupied", "Maintenance" };
        var roomStatus = status ?? "Available";
        if (!validStatuses.Contains(roomStatus))
        {
            throw new ArgumentException($"Invalid status. Valid values: {string.Join(", ", validStatuses)}");
        }

        // Create and add the room
        var room = new Room
        {
            RoomTypeId = roomTypeId,
            RoomNumber = roomNumber,
            Floor = floor,
            Status = roomStatus,
            CreatedDate = DateTime.UtcNow
        };

        return await _roomRepository.AddAsync(room);
    }

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
                CreatedDate = DateTime.UtcNow
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
        room.UpdatedDate = DateTime.UtcNow;

        await _roomRepository.UpdateAsync(room);
        return room;
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        await _roomRepository.DeleteAsync(roomId);
    }
}
