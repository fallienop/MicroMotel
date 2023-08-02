using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IMealRService
    {
        public Task<Response<MealR>> GetMealRById(int id);
        public Task<Response<List<MealR>>> GetAllMealRs();
        public Task<Response<NoContent>> CreateReservation(MealR MealR);
        public Task<Response<NoContent>> UpdateReservation(MealR MealR);
        public Task<Response<NoContent>> DeleteMealReservation(int id);
    }
}
