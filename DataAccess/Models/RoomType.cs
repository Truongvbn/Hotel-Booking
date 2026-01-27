namespace DataAccess.Models;

public class RoomType
{
    public int RoomTypeId { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty; // Single, Double, Suite, etc.
    public string? Description { get; set; }
    public int Capacity { get; set; } = 2;
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    // Navigation properties
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
