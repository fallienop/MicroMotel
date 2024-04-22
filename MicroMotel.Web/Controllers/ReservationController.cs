using MicroMotel.Shared.Services;
using MicroMotel.Web.Models;
using MicroMotel.Web.Models.FakePayment;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using MicroMotel.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Text;
using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MicroMotel.Web.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMotelService _motelService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userservice;

        public ReservationController(IReservationService reservationService, ISharedIdentityService sharedIdentityService, IMotelService motelService, IPaymentService paymentService, IUserService userservice)
        {
            _reservationService = reservationService;
            _sharedIdentityService = sharedIdentityService;
            _motelService = motelService;
            _paymentService = paymentService;
            _userservice = userservice;
        }

        public async Task<IActionResult> Room(int id,int propertyid)
        {
            var reservations = await _reservationService.GetAllByRoomId(id);
            var user = await _userservice.GetUser();
            HttpContext.Session.SetString("userbudget", user.Budget.ToString()); 
            HttpContext.Session.SetString("roomprice", (await _motelService.GetRoomById(id)).Price.ToString()); 



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
            var user = await _userservice.GetUser();

            var validator = new RoomRCreateValidator(_reservationService);
            var result=await validator.ValidateAsync(rci);
                var reservations = await _reservationService.GetAllByRoomId(rci.RoomId);
                ViewData["reservs"] = reservations;

            
            if(result.IsValid) 
            {
                


                var room = await _motelService.GetRoomById(rci.RoomId);
                decimal total = ((decimal)(((rci.ReservEnd - rci.ReservStart).TotalMinutes) / 60) * room.Price);
          
             

                if (user.Budget < total)
                {
                    ModelState.AddModelError(string.Empty, "insufficient balance");
                    HttpContext.Session.SetString("requiredmoney", (total - user.Budget).ToString());
                    return RedirectToAction(nameof(Payment) );

                    //ViewData["TotalAmount"] = total;
                    //return View("InsufficientBalanceConfirmation", rci);
                }
                else
                {
                    HttpContext.Session.SetString("propid", rci.PropertyId.ToString());
                    HttpContext.Session.SetString("reservstart", (rci.ReservStart.ToString()));
                    HttpContext.Session.SetString("reservend", (rci.ReservEnd).ToString());
                    rci.TotalPrice= total;
                    var resp = await _reservationService.NewRoomReservation(rci);
                    HttpContext.Session.SetString("resp", resp.ToString());
                    user.Budget -= total;
                    UserUpdateModel usr = new () { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

                    await _userservice.AddBalance(usr);
                }
            }
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }

                return View(rci);
            }
          
           return RedirectToAction("doyouwant");
        } 

        public IActionResult doyouwant()
        {
            return View();
        }

        public async Task<IActionResult> MealR()
        {
            try
            {

                ViewBag.mrstart = Convert.ToDateTime(HttpContext.Session.GetString("reservstart"));
            ViewBag.mrend  = Convert.ToDateTime(HttpContext.Session.GetString("reservend"));
                var meals = await _motelService.GetAllMealsByPropertyId(int.Parse(HttpContext.Session.GetString("propid")));
            ViewData["meals"] = meals;
            MealRCreateInput mci = new() { RoomRId = int.Parse(HttpContext.Session.GetString("resp")) };

            return View(mci);
            }
            catch
            {
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
                        decimal total = prices.Sum();
                        var user = await _userservice.GetUser();

                        if (user.Budget < total)
                        {
                            ModelState.AddModelError(string.Empty, "insufficient balance");
                            HttpContext.Session.SetString("requiredmoney", (total - user.Budget).ToString());
                            return RedirectToAction(nameof(Payment));

                        }
                        else
                        {
                            user.Budget -= total;
                            UserUpdateModel usr = new () { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

                            await _userservice.AddBalance(usr);

                            var r = await _reservationService.NewMealReservation(mci);
                        }
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

               // TempData["prices"] = prices;
                
               return Json(new { success = true, redirectUrl = Url.Action("Payment", "Reservation") });
            }
            catch 
            {
          
                string referer = Request.Headers["Referer"].ToString();
                return Redirect(referer); 
            }
        }


        


            public IActionResult Payment()
            {
              
                PaymentInput payment = new ()
                {
                   
                    TotalPrice = 0
               };
            decimal? reqmoney =Convert.ToDecimal( HttpContext.Session.GetString("requiredmoney")); 
                return View(payment);
            }



            [HttpPost]
            public async Task<IActionResult> Payment(PaymentInput payment)
            {

              HttpContext.Session.SetString("paymentobj",JsonSerializer.Serialize(payment));
            var r = await _paymentService.TestCard(payment);
                if (r)
                {
                    var card = await _paymentService.GetCard(payment.CardNumber);
                var amount = payment.TotalPrice;
                HttpContext.Session.SetString("Amount", amount.ToString());
                    var randomcode = SendEmail(card.Email);
                    TempData["RandomCode"] = randomcode;
                    return RedirectToAction("PaymentConfirm");
                }

            
                return View();
            }

        public IActionResult PaymentConfirm()
        {
            string random = TempData["RandomCode"] as string;
            int rand = int.Parse(random);
            VerificationCode vc = new () { Code=0 };
            HttpContext.Session.SetInt32("randomcode", rand);
            return View(vc);
        }
        
        [HttpPost]
        public async Task<IActionResult> PaymentConfirm(VerificationCode vc)
        {
            int? rand = HttpContext.Session.GetInt32("randomcode");
            bool isCodeCorrect = rand == vc.Code;


            if (isCodeCorrect)
            {
                var user = await _userservice.GetUser();
                decimal money = Convert.ToDecimal(HttpContext.Session.GetString("Amount"));
                user.Budget += money;
                UserUpdateModel usr = new () { Budget=user.Budget,City=user.City,Email=user.Email,Username=user.UserName };
                var paymentobject = JsonSerializer.Deserialize<PaymentInput>(HttpContext.Session.GetString("paymentobj"));
                await _paymentService.ReceivePayment(paymentobject);
                await _userservice.AddBalance(usr);
                return RedirectToAction("Index","Home");
            }
            else
            {
            
                ModelState.AddModelError("verificationCode", "Verification code is wrong");
                return View();
            }
        }
        private static string SendEmail(string email)
        {
            Random r = new Random();
            string random6 = (r.Next(100000, 999999)).ToString();
            MailMessage mymessage=new ();
            SmtpClient client = new();
            client.Credentials = new System.Net.NetworkCredential("******@outlook.com", "*********");
            client.Port = 587;
            client.Host = "smtp-mail.outlook.com";
            client.EnableSsl = true;
            mymessage.To.Add(email);
            mymessage.From = new MailAddress("sahin.b.03@outlook.com");
            mymessage.Subject = "Confirmation Code";
            mymessage.Body = $"Your payment confirmation code : {random6}";
            client.Send(mymessage);
            return random6;
        }
    



        public async Task<IActionResult> getownreservs()
        {
           var onehourerror= HttpContext.Session.GetString("timeonehour");

            if (!onehourerror.IsNullOrEmpty())
            {
            var modelerror = JsonSerializer.Deserialize<ModelStateDictionary>(onehourerror);
                var modelErrorDictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(onehourerror);

                foreach (var entry in modelErrorDictionary)
                {
                    foreach (var errorMessage in entry.Value)
                    {
                        ModelState.AddModelError(entry.Key, errorMessage);
                    }
                }
            }
            var reservs = await _reservationService.GetAllByUserId(_sharedIdentityService.getUserId);
            foreach(var res in reservs)
            {
                res.PropertyName = (await _motelService.GetPropertybyId(res.PropertyId)).Name;

                if (res.MealRs.Any())
                {
                    foreach (var meal in res.MealRs)
                    {
                        meal.MealName = (await _motelService.GetMealById(meal.MealId)).Name;
                    }

                   
                }
                

            }
            
            return View(reservs); 
        }
        public async Task<IActionResult> CancelRoomReserv(int id)
        {
            var roomres = await _reservationService.GetRoomRById(id);
            
         var r=  await _reservationService.DeleteRoomReservation(id);
            var user = await _userservice.GetUser();
            decimal totalmealprice = 0;
            if (roomres.MealRs.Any())
            {
                foreach(var mealres in roomres.MealRs)
                {
                    var meal = await _motelService.GetMealById(mealres.MealId);
                    totalmealprice += meal.Price;
                }
            }
            user.Budget += (roomres.TotalPrice+totalmealprice) * (decimal)0.9;
            UserUpdateModel usr = new() { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

            await _userservice.AddBalance(usr);
            return RedirectToAction("getownreservs");

        }

        public async Task<IActionResult> CancelMealReserv(int id)
        {
            var mealres = await _reservationService.GetMealRById(id);

          var hour = (mealres.ReservationDate-DateTime.Now).TotalHours;

            if (hour < 1)
            {
                ModelStateDictionary model=new ModelStateDictionary();
                model.AddModelError("timeonehour", "You cannot cancel meal reservation less than one hour before the reservation time");
                string modelasstring = JsonSerializer.Serialize(model);
                HttpContext.Session.SetString("timeonehour",modelasstring);
                return RedirectToAction(nameof(getownreservs));
            }
            else
            {
                var r = await _reservationService.DeleteMealReservation(id);
                var user = await _userservice.GetUser();


                var meal = await _motelService.GetMealById(mealres.MealId);



                user.Budget += meal.Price * (decimal)0.9;
                UserUpdateModel usr = new() { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

                await _userservice.AddBalance(usr);
                return RedirectToAction("getownreservs");
            }

        }
    }
    
  

    public class SelectedMealViewModel
    {
        public int id { get; set; }  
        public DateTime date { get; set; }
        public int roomrid { get; set; } 

    }
}
