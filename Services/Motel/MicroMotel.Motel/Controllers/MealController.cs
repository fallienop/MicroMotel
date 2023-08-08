using MicroMotel.Services.Motel.DTOs.MealDTOs;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Services.Motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : CustomControllerr
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("property/{id}")]
        public async Task<IActionResult> GetAllByProperty(int id)
        {
            var meals = await _mealService.GetAllMeals(id);
            return CustomActionResult(meals);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var meal=await _mealService.GetMealById(id);
            return CustomActionResult(meal);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteById(int id)
        {
            var resp=await _mealService.DeleteMealById(id);
            return CustomActionResult(resp);
        }
        [HttpPut]
        public async Task<IActionResult> Update (MealUpdateDTO mud)
        {
            var resp = await _mealService.UpdateMeal(mud);
            return CustomActionResult(resp);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MealCreateDTO mcd)
        {
            var resp = await _mealService.CreateNewMeal(mcd);
            return CustomActionResult(resp);
        }
    }
}
