using MicroMotel.Web.Models.Motel.Meal;
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
        public IActionResult AddProperty()
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
            ViewBag.propid = id;
            return View(values);

        }
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var values = await _MotelService.GetPropertybyId(id);
            ViewBag.propid = id;
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
        public async Task<IActionResult> AddRoom(int id)
        {
            var res = await _MotelService.GetAllPropertiesAsync();
            var prop = res.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.FloorCount = prop.FloorCount;
            var roomcreate = new RoomCreateInput()
            {
                PropertyId = id
            };
            return View(roomcreate);
        }
        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomCreateInput rci)
        {

            await _MotelService.CreateNewRoom(rci);
            return RedirectToAction("propertylist");

        }

        public async Task<IActionResult> UpdateRoom(int id,int propid)
        {
            //int sub = link.IndexOf('_');
            //int id=int.Parse(link.Substring(0, sub));
            //int propid=int.Parse(link.Substring(sub+1));

            //var res = await _MotelService.GetAllPropertiesAsync();
            //var prop = res.Where(x => x.Id == propid).FirstOrDefault();

            //ViewBag.FloorCount = prop.FloorCount;
            //var room = await _MotelService.GetRoomById(id);


            //return View(room);

            var res = await _MotelService.GetAllPropertiesAsync();
            var prop = res.FirstOrDefault(x => x.Id == propid);

            if (prop != null)
            {
                ViewBag.FloorCount = prop.FloorCount;
                var room = await _MotelService.GetRoomById(id);
                return View(room);
            }

            // Handle if property not found
            return RedirectToAction("PropertyNotFound");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRoom(RoomUpdateModel rum)
        {
            await _MotelService.UpdateRoom(rum);
            return RedirectToAction("RoomDetails", new { Id = rum.Id });


        }


        public async Task<IActionResult> PropertyWithMeals(int id)
        {
            var meals = await _MotelService.GetAllMealsByPropertyId(id);
            ViewBag.propid = id;
            return View(meals);
        }

        public async Task<IActionResult> MealDetails(int id)
        {
            var meal=await _MotelService.GetMealById(id);
            return View(meal);
        }

        public async Task<IActionResult> AddMeal(int id)
        {
            var mealcreate = new MealCreateInput()
            {
                PropertyId = id
            };
            
            return View(mealcreate);
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal(MealCreateInput mci)
        {
            await _MotelService.CreateNewMeal(mci);

            return RedirectToAction(nameof(PropertyWithMeals));
        }
    }
}
