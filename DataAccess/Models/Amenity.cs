namespace DataAccess.Models;

public class Amenity
{
    public int AmenityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; } // Icon class name for UI
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation property
    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
}
