using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AllergyNutritionPlatform.Data;
using AllergyNutritionPlatform.Models;

namespace AllergyNutritionPlatform.Controllers;

public class HomeController : Controller
{
   private readonly ApplicationDbContext _context;
private readonly ILogger<HomeController> _logger;

public HomeController(
    ApplicationDbContext context,
    ILogger<HomeController> logger)
{
    _context = context;
    _logger = logger;
}

    public IActionResult Index()
{
   var userId = HttpContext.Session.GetInt32("UserId");

ViewBag.ProductCount = _context.Products.Count();
ViewBag.RestaurantCount = _context.Restaurants.Count();

if (userId != null)
{
    ViewBag.TotalAllergies = _context.UserAllergies
        .Count(x => x.UserId == userId.Value);

    ViewBag.FavoriteCount = _context.Favorites
        .Count(x => x.UserId == userId.Value);
}
else
{
    ViewBag.TotalAllergies = 0;
    ViewBag.FavoriteCount = 0;
}
    return View();
}



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
