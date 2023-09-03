using MicroMotel.Shared.Services;
using MicroMotel.Web.Models.Reservation.MealR;

namespace MicroMotel.Web.Models.Reservation.RoomR
{
    public class RoomRCreateInput
    {

        public int PropertyId { get; set; }
        public int RoomId { get; set; }
        public DateTime ReservStart { get; set; }
        public DateTime ReservEnd { get; set; }
        public ICollection<MealRCreateInput> MealRs { get; set; }
        public string UserID { get; set; } 
       
    }
}
