
using MicroMotel.Shared.Services;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using MicroMotel.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MicroMotel.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMotelService _motelService;

        public ReservationController(IReservationService reservationService, ISharedIdentityService sharedIdentityService, IMotelService motelService)
        {
            _reservationService = reservationService;
            _sharedIdentityService = sharedIdentityService;
            _motelService = motelService;
        }

        public async Task<IActionResult> Room(int id,int propertyid)
        {
            var reservations = await _reservationService.GetAllByRoomId(id);
            RoomRCreateInput rci = new()
            {
                RoomId = id,
                PropertyId = propertyid,
                UserID = _sharedIdentityService.getUserId
                
            };
            ViewData["reservs"] = reservations;
            return View(rci);
        }

        [HttpPost]
        public async Task<IActionResult> Room(RoomRCreateInput rci)
        {
            
            var validator = new RoomRCreateValidator(_reservationService);
            var result=await validator.ValidateAsync(rci);
                var reservations = await _reservationService.GetAllByRoomId(rci.RoomId);
                ViewData["reservs"] = reservations;
            if(result.IsValid) 
            {
                var resp =await _reservationService.NewRoomReservation(rci);
               
                TempData["rid"] =resp;
                TempData["propid"] = rci.PropertyId;
            }
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                return View(rci);
            }
          
           return RedirectToAction(nameof(doyouwant));
        } 

        public IActionResult doyouwant()
        {
            return View();
        }

        public async Task<IActionResult> MealR()
        {
            var meals = await _motelService.GetAllMealsByPropertyId(int.Parse(TempData["propid"].ToString()));
            ViewData["meals"] = meals;
            MealRCreateInput mci = new() { RoomRId = int.Parse(TempData["rid"].ToString()) };

            return View(mci);
        }
        [HttpPost]
        public async  Task<IActionResult> MealR(MealRCreateInput mci)
        {
            var validator=new MealRCreateValidator(_reservationService);
            var result = await validator.ValidateAsync(mci);
           
            if (result.IsValid)
            {
                 await _reservationService.NewMealReservation(mci);

            }
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                return View(mci);
            }

            return RedirectToAction(nameof(HomeController.Index), "home");
        }
    }
}
