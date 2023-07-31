using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IMealRService
    {
        public Task<MealR> GetMealRById(int id);
        public Task<List<MealR>> GetAllMealRs();
        public Task<NoContent> CreateReservation(MealR MealR);
        public Task<NoContent> UpdateReservation(MealR MealR);
        public Task<NoContent> DeleteMealReservation(int id);
    }
}
