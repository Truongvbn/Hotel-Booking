namespace DataAccess.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public int Rating { get; set; } = 5; // 1-5 stars
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Hotel Hotel { get; set; } = null!;
}
