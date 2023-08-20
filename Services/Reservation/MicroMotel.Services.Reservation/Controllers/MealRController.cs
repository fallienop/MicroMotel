using MicroMotel.Services.Reservation.DTOs.MealRDTOs;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Services.Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealRController : CustomControllerr
    {
        private readonly IMealRService _mealrservice;

        public MealRController(IMealRService mealrservice)
        {
            _mealrservice = mealrservice;
        }

        [HttpGet("prop/{id}")]
        public async Task<IActionResult> GetMealrByProp(int propid)
        {
            var resp = await _mealrservice.GetAllMealsByProperty(propid);
            return CustomActionResult(resp);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetMealrByRoom(int roomid)
        {
            var resp = await _mealrservice.GetAllMealsByRoom(roomid);
            return CustomActionResult(resp);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _mealrservice.GetAllMealRs();
            return CustomActionResult(resp);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var resp = await _mealrservice.GetMealRById(id);
            return CustomActionResult(resp);
        }

        [HttpPost]
        public async Task<IActionResult> newres(MealRCreateDTO mealr)
        {
            var resp = await _mealrservice.CreateReservation(mealr);
            return CustomActionResult(resp);

        }
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _mealrservice.DeleteMealReservation(id);
            return CustomActionResult(res);
        }

    }
}
