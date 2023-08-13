using MicroMotel.Web.Models.Motel.Property;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMotelService _MotelService;

        public AdminController(IMotelService motelService)
        {
            _MotelService = motelService;
        }

        public async Task<IActionResult> PropertyList()
        {

            var values = await _MotelService.GetAllPropertiesAsync();
            return View(values);
        
        }
        public  IActionResult AddProperty()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProperty(PropertyCreateInput pci)
        {
            await _MotelService.CreateProperty(pci);
            return RedirectToAction("propertylist");

        }
        public async Task<IActionResult> PropertyWithRooms(int id)
        {

            var values = await _MotelService.GetPropertyWithRoomsAsync(id);
            return View(values);
        
        }
        public async Task<IActionResult> RoomDetails(int id)
        {
            var values = await _MotelService.GetRoomById(id);
            return View(values);
        }
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _MotelService.DeleteRoom(id);
            return RedirectToAction("propertylist");
        }
        public IActionResult AddRoom()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomCreateInput rci)
        {
            await _MotelService.CreateNewRoom(rci);
            return RedirectToAction("propertylist");

        }
    }
}
