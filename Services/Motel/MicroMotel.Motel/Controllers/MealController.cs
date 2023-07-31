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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var meals=await _mealService.GetAllMeals();
            return CustomActionResult(meals);
        }
        public async Task<IActionResult> GetById(int id)
        {
            var meal=await _mealService.GetMealById(id);
            return CustomActionResult(meal);
        }
        public async Task<IActionResult> DeleteById(int id)
        {
            var resp=await _mealService.DeleteMealById(id);
            return CustomActionResult(resp);
        }
        public async Task<IActionResult> Update (MealUpdateDTO mud)
        {
            var resp = await _mealService.UpdateMeal(mud);
            return CustomActionResult(resp);
        }
        public async Task<IActionResult> Create(MealCreateDTO mcd)
        {
            var resp = await _mealService.CreateNewMeal(mcd);
            return CustomActionResult(resp);
        }
    }
}
