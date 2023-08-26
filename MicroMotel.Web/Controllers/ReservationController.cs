using MicroMotel.Shared.Services;
using MicroMotel.Web.Models;
using MicroMotel.Web.Models.FakePayment;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Interface;
using MicroMotel.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.Json;

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

                HttpContext.Session.SetString("propid", rci.PropertyId.ToString()); 
               HttpContext.Session.SetString("resp", resp.ToString()); 
               HttpContext.Session.SetString("reservstart", (rci.ReservStart.ToString()));
               HttpContext.Session.SetString("reservend", (rci.ReservEnd).ToString());
                var room = await _motelService.GetRoomById(rci.RoomId);
                decimal total = ((decimal)(((rci.ReservEnd - rci.ReservStart).TotalMinutes) / 60) * room.Price);
                var user = await _userservice.GetUser();
                if (user.Budget < total)
                {
                    ModelState.AddModelError(string.Empty, "insufficient balance");
                    return RedirectToAction("getusersets", "auth");

                }
                else
                {
                    user.Budget -= total;
                    UserUpdateModel usr = new UserUpdateModel() { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

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
                            return RedirectToAction("getusersets", "auth");

                        }
                        else
                        {
                            user.Budget -= total;
                            UserUpdateModel usr = new UserUpdateModel() { Budget = user.Budget, City = user.City, Email = user.Email, Username = user.UserName };

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






            public async Task<IActionResult> Payment()
            {
                //decimal total=0;
             //var roomid= int.Parse(HttpContext.Session.GetString("resp"));
                //var reservstart= Convert.ToDateTime(HttpContext.Session.GetString("reservstart"));
                //var reservend= Convert.ToDateTime(HttpContext.Session.GetString("reservend"));
                //var room = await _motelService.GetRoomById(roomid);
              //int h=(int) (reservend - reservstart).TotalHours;
                //if (TempData["prices"] != null)
                //{
                    //var totalmealprice = TempData["prices"] as List<decimal>;
                    //foreach (var mealprice in totalmealprice)
                    //{
                        //total += mealprice;
                    //}
                //}
                //total += room.Price * h;
                PaymentInput payment = new PaymentInput()
                {
                    // TotalPrice = total,
                    TotalPrice = 0
               };
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
            VerificationCode vc = new VerificationCode() { Code=0 };
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
                UserUpdateModel usr = new UserUpdateModel() { Budget=user.Budget,City=user.City,Email=user.Email,Username=user.UserName };
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
        private string SendEmail(string email)
        {
            Random r = new Random();
            string random6 = (r.Next(100000, 999999)).ToString();
            MailMessage mymessage=new MailMessage();
            SmtpClient client = new();
            client.Credentials = new System.Net.NetworkCredential("sahin.b.03@outlook.com", "1Parola7");
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

    }
    

    public class SelectedMealViewModel
    {
        public int id { get; set; }  
        public DateTime date { get; set; }
        public int roomrid { get; set; } 

    }
}
