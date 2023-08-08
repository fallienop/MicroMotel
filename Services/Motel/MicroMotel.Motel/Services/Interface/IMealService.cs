using MicroMotel.Motel.DTOs.RoomDTOs;
using MicroMotel.Services.Motel.DTOs.MealDTOs;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Motel.Services.Interface
{
    public interface IMealService
    {
        Task<Response<List<MealDTO>>> GetAllMeals(int propertyid);
        Task<Response<MealDTO>> GetMealById(int id);
        Task<Response<NoContent>> CreateNewMeal(MealCreateDTO mcd);
        Task<Response<NoContent>> UpdateMeal(MealUpdateDTO mud);
        Task<Response<NoContent>> DeleteMealById(int id);
    }
}
