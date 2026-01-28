using System.ComponentModel.DataAnnotations;

namespace HotelBooking.ViewModels;

/// <summary>
/// ViewModel for editing user profile information
/// </summary>
public class EditProfileViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    // Read-only display fields
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for changing password
/// </summary>
public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your new password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for admin user list
/// </summary>
public class AdminUserListViewModel
{
    public IEnumerable<AdminUserItem> Users { get; set; } = new List<AdminUserItem>();
    public string? RoleFilter { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int InactiveUsers { get; set; }
}

/// <summary>
/// Individual user item for admin list
/// </summary>
public class AdminUserItem
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? HotelName { get; set; }
}

/// <summary>
/// ViewModel for admin editing a user
/// </summary>
public class AdminEditUserViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [Display(Name = "Role")]
    public string Role { get; set; } = "Customer";

    [Display(Name = "Account Status")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Assigned Hotel")]
    public int? HotelId { get; set; }

    // For dropdown population
    public IEnumerable<string> AvailableRoles { get; set; } = new[] { "Customer", "HotelOwner", "Admin" };
    public IEnumerable<HotelDropdownItem> AvailableHotels { get; set; } = new List<HotelDropdownItem>();

    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Simple hotel item for dropdown
/// </summary>
public class HotelDropdownItem
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
}
