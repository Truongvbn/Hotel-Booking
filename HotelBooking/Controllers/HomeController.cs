using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;
using HotelBooking.ViewModels;
using Services.Interfaces;

namespace HotelBooking.Controllers;

public class HomeController : Controller
{
    private readonly IHotelService _hotelService;

    public HomeController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public async Task<IActionResult> Index()
    {
        var cities = await _hotelService.GetAllCitiesAsync();
        var model = new HotelSearchResultViewModel
        {
            SearchCriteria = new SearchViewModel
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                Guests = 2
            },
            AvailableCities = cities
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
