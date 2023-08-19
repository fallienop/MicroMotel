﻿using MicroMotel.Shared.Services;
using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Motel.Property;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IMotelService _MotelService;
        private readonly IReservationService _reservationservice;
        private readonly IUserService _userservice;

        public AdminController(IMotelService motelService, IReservationService reservationservice, IUserService userservice)
        {
            _MotelService = motelService;
            _reservationservice = reservationservice;
            _userservice = userservice;
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
            if (TempData.ContainsKey("PropertyId") && TempData["PropertyId"] != null)
            {
                id = (int)TempData["PropertyId"];
            }
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

            return RedirectToAction(nameof(PropertyWithMeals),new {Id=mci.PropertyId});
        }

        public async Task<IActionResult> DeleteMeal(int id,int propid)
        {
            await _MotelService.DeleteMeal(id);
            TempData["PropertyId"] = propid;

            return RedirectToAction(nameof(PropertyWithMeals),new {Id=propid});

        }

        public async Task<IActionResult> UpdateMeal(int id)
        {
            var meal = await _MotelService.GetMealById(id);


            return View(meal);
            
        }

        

        [HttpPost]
        public async Task<IActionResult> UpdateMeal(MealUpdateModel mum)
        {
            await _MotelService.UpdateMeal(mum);
            return RedirectToAction(nameof(MealDetails), new { Id = mum.Id });
        }


        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _MotelService.DeleteProperty(id);
            return RedirectToAction(nameof(PropertyList));

        }

        public async Task<IActionResult> UpdateProperty(int id)
        {
            var prop = await _MotelService.GetPropertybyId(id);
            return View(prop);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProperty(PropertyUpdateModel pum)
        {
            await _MotelService.UpdateProperty(pum);
            return RedirectToAction(nameof(PropertyDetails),new {Id=pum.Id});
        }

        public async Task<IActionResult> AllRoomReservations()
        {
            var reservs = await _reservationservice.GetAll();
            //    reservs.ForEach(async (x) => x.UserName = (await _userservice.getusername(x.UserID)).UserName) ;
            var username = await _userservice.getusername("979ea7d9-265e-456d-bf34-bd6157e8e60d");
            return View(reservs);
        }

        public async Task<IActionResult> DeleteRoomReserv(int id)
        {
            await _reservationservice.DeleteRoomReservation(id);
            return RedirectToAction(nameof(AllRoomReservations));
        }

    }
}