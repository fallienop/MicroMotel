using MicroMotel.Web.Models;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MicroMotel.Web.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMotelService _motelService;

        public HomeController(ILogger<HomeController> logger, IMotelService motelService)
        {
            _logger = logger;
            _motelService = motelService;
           
        }

        public async Task<IActionResult> Index()
        {
        
            return View(await _motelService.GetAllPropertiesAsync());
        }
        public async Task<IActionResult> Rooms(int id)
        {

            return View(await _motelService.GetPropertyWithRoomsAsync(id));
        }

        public async Task<IActionResult> Meals(int id)
        {
            return View(await _motelService.GetAllMealsByPropertyId(id));

        }

        public async Task<IActionResult> MealDetails(int id)
        {
            return View(await _motelService.GetMealById(id));
        }
        public async Task<IActionResult> RoomDetail(int id)
        {
            var room = await _motelService.GetRoomById(id);
            return View(room);
        }
        public async Task<IActionResult> PropertyDetail(int id)
        {
            var property = await _motelService.GetPropertybyId(id);
            return View(property);
        }
        public IActionResult Contact()
        {
            return View();
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
}