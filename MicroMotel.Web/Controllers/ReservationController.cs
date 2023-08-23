
using MicroMotel.Shared.Services;
using MicroMotel.Web.Models.FakePayment;
using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using MicroMotel.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.Mail;

namespace MicroMotel.Web.Controllers
{
        [ResponseCache(NoStore = true, Duration = 0)]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMotelService _motelService;
        private readonly IPaymentService _paymentService;

        public ReservationController(IReservationService reservationService, ISharedIdentityService sharedIdentityService, IMotelService motelService, IPaymentService paymentService)
        {
            _reservationService = reservationService;
            _sharedIdentityService = sharedIdentityService;
            _motelService = motelService;
            _paymentService = paymentService;
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
               
                TempData["propid"] = rci.PropertyId;
                TempData["rid"] =resp;
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
            List<decimal> prices = new();
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
                    var meal = await _motelService.GetMealById(mci.MealId);
                    prices.Add(meal.Price);
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

                TempData["prices"] = prices;
                return RedirectToAction("Index","Home");
                // İşlemler başarılıysa veya hata olmadıysa başka bir sayfaya yönlendirme yapabilirsiniz.
            }
            catch 
            {
                // Hata durumunda önceki sayfaya veya başka bir sayfaya yönlendirme yapabilirsiniz.
                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer); 
            }
        }






        public async Task<IActionResult> Payment()
        {
            decimal total=0;
         var roomid=(int)TempData["rid"];
       var reservstart=(DateTime)TempData["rrstart"] ;
           var reservend=(DateTime)TempData["rrend"] ;
            var room = await _motelService.GetRoomById(roomid);
          int h=(int) (reservend - reservstart).TotalHours;
            if (TempData["prices"] != null)
            {
                var totalmealprice = TempData["prices"] as List<decimal>;
                foreach (var mealprice in totalmealprice)
                {
                    total += mealprice;
                }
            }
            total += room.Price * h;
           PaymentInput payment=new PaymentInput()
           {
               TotalPrice = total,
              
           };
            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(PaymentInput payment)
        {
           var r= await _paymentService.ReceivePayment(payment);
            if (r)
            {
                var card = await _paymentService.GetCard(payment.CardNumber);
                var randomcode = SendEmail(card.Email);
                TempData["RandomCode"] = randomcode;
                return RedirectToAction("PaymentConfirm");
            }

            
            return View();
        }

        public IActionResult PaymentConfirm()
        {
            string random = TempData["RandomCode"] as string;
            return View(random);
        
        }
        [HttpPost]
        public IActionResult PaymentConfirm(string random)
        {
            return View();
        
        
        }
            private string SendEmail(string email)
        {
            Random r = new Random();
            string random6 = (r.Next(100000, 999999)).ToString();
            MailMessage mymessage=new MailMessage();
            SmtpClient client = new();
            client.Credentials = new System.Net.NetworkCredential("micromotelfp@outlook.com", "1_Micromotel");
            client.Port = 587;
            client.Host = "smtp-mail.outlook.com";
            client.EnableSsl = true;
            mymessage.To.Add(email);
            mymessage.From = new MailAddress("micromotelfp@outlook.com");
            mymessage.Subject = "Confirmation Code";
            mymessage.Body = $"Your payment confirmation code : {random6}";

            return random6;
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