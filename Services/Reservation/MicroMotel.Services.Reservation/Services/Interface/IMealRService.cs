using MicroMotel.Services.Reservation.DTOs.MealRDTOs;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IMealRService
    {
        Task<Response<List<MealRDTO>>> GetAllMealsByProperty(int propertyid);
        Task<Response<List<MealRDTO>>> GetAllMealsByRoom(int roomid);
        public Task<Response<MealRDTO>> GetMealRById(int id);
        public Task<Response<List<MealRDTO>>> GetAllMealRs();
        public Task<Response<NoContent>> CreateReservation(MealRCreateDTO MealR);
        public Task<Response<NoContent>> DeleteMealReservation(int id);
    }
}
