
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
        [ResponseCache(NoStore = true, Duration = 0)]
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
                TempData["rrstart"] = rci.ReservStart;
                TempData["rrend"] = rci.ReservEnd;
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
            try
            {
                ViewBag.mrstart = TempData["rrstart"];
            ViewBag.mrend  = TempData["rrend"];
            var meals = await _motelService.GetAllMealsByPropertyId(int.Parse(TempData["propid"].ToString()));
            ViewData["meals"] = meals;
            MealRCreateInput mci = new() { RoomRId = int.Parse(TempData["rid"].ToString()) };

            return View(mci);
            }
            catch
            {
                // Hata durumunda önceki sayfaya veya başka bir sayfaya yönlendirme yapabilirsiniz.
                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer);
            }
        }
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> MealR([FromBody] List<SelectedMealViewModel> selectedMeals)
        {
            try
            {
                var validator = new MealRCreateValidator(_reservationService);
               
                foreach (var selectedMeal in selectedMeals)
                {
                    MealRCreateInput mci = new()
                    {
                        MealId = selectedMeal.id,RoomRId=selectedMeal.roomrid,ReservationDate=selectedMeal.date
                    };

                    var result = await validator.ValidateAsync(mci);

                    if (result.IsValid)
                    {
                     var r=   await _reservationService.NewMealReservation(mci);

                        }
                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.ErrorMessage);
                        }

                        return View(mci);
                    }


                }
                    return RedirectToAction(nameof(HomeController.Index), "home");

                // İşlemler başarılıysa veya hata olmadıysa başka bir sayfaya yönlendirme yapabilirsiniz.
            }
            catch 
            {
                // Hata durumunda önceki sayfaya veya başka bir sayfaya yönlendirme yapabilirsiniz.
                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer);
            }
        }
    }

    public class SelectedMealViewModel
    {
        public int id { get; set; }  // Seçilen yemeğin ID'si
        public DateTime date { get; set; } // Seçilen tarih
        public int roomrid { get; set; } // Seçilen tarih

    }
}

#region oldmealr
//[httppost]
//public async  task<iactionresult> mealr(mealrcreateinput mci)
//{
//    try
//    {

//        var validator = new mealrcreatevalidator(_reservationservice);
//        var result = await validator.validateasync(mci);

//        if (result.isvalid)
//        {
//            await _reservationservice.newmealreservation(mci);

//        }
//        if (!result.isvalid)
//        {
//            foreach (var error in result.errors)
//            {
//                modelstate.addmodelerror("", error.errormessage);
//            }

//            return view(mci);
//        }

//        return redirecttoaction(nameof(homecontroller.index), "home");
//    }
//    catch
//    {
//        string referer = request.headers["referer"].tostring();
//        return redirect(referer);
//    }
//} 
#endregion