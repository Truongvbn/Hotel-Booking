namespace DataAccess.Models;

public class Hotel
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public decimal StarRating { get; set; } = 0;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
