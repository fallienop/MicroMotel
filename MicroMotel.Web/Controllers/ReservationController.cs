
using MicroMotel.Shared.Services;
using MicroMotel.Web.Models.Motel.Room;
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

        public ReservationController(IReservationService reservationService, ISharedIdentityService sharedIdentityService)
        {
            _reservationService = reservationService;
            _sharedIdentityService = sharedIdentityService;
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
         var r=   await _reservationService.NewRoomReservation(rci);

            }
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                return View(rci);
            }
            return View();
           // return RedirectToAction("");
        } 
    }
}
