namespace DataAccess.Models;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = "Customer"; // Customer, HotelOwner, Admin
    public int? HotelId { get; set; } // Only for HotelOwner role
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Hotel? Hotel { get; set; } // Hotel managed by this user (if HotelOwner)
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    // Computed property
    public string FullName => $"{FirstName} {LastName}".Trim();
}
