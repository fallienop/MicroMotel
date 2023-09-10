using MicroMotel.Services.Reservation.DTOs.MealRDTOs;
using MicroMotel.Services.Reservation.Models;

namespace MicroMotel.Services.Reservation.DTOs.RoomRDTOs
{
    public class RoomRCreateDTO
    {
        
        public int PropertyId { get; set; }
        public int RoomId { get; set; }
        public DateTime ReservStart { get; set; }
        public DateTime ReservEnd { get; set; }
        public ICollection<MealRCreateDTO>? MealRs { get; set; }
        public string? UserID { get; set; }

        public decimal? TotalPrice { get; set; }

    }
}
