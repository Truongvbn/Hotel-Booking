using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HotelBooking.ViewModels;
using Services.Interfaces;

namespace HotelBooking.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: /User/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    // POST: /User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(model.RememberMe ? 30 : 1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect based on role
            if (user.Role == "Admin" || user.Role == "Staff")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Login failed. Please try again later.");
            return View(model);
        }
    }

    // GET: /User/Register
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new RegisterViewModel());
    }

    // POST: /User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var user = await _userService.RegisterAsync(
                model.Email,
                model.Password,
                model.FirstName,
                model.LastName,
                model.PhoneNumber);

            // Auto-login after registration
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            TempData["Success"] = "Registration successful! Welcome to our hotel booking system.";
            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException ex)
        {
            // Known errors like "Email already exists"
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Registration failed. Please try again later.");
            return View(model);
        }
    }

    // POST: /User/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // GET: /User/Profile
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToAction("Login");
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: /User/Edit
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToAction("Login");
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel
        {
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Address = user.Address ?? string.Empty
        };

        return View(model);
    }

    // POST: /User/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var updatedUser = await _userService.UpdateProfileAsync(
                userId,
                model.FirstName,
                model.LastName,
                model.PhoneNumber,
                model.Address);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
            return View(model);
        }
    }

    // GET: /User/AccessDenied
    public IActionResult AccessDenied()
    {
        return View();
    }
}
