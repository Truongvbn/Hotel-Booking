using System.ComponentModel.DataAnnotations;

namespace HotelBooking.ViewModels;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    [StringLength(20)]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Address")]
    [StringLength(255)]
    public string? Address { get; set; }
}
