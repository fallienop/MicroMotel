using MicroMotel.Web.Models.Reservation.MealR;

namespace MicroMotel.Web.Models.Reservation.RoomR
{
    public class RoomRViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RoomId { get; set; }
        public DateTime ReservStart { get; set; }
        public DateTime ReservEnd { get; set; }
        public ICollection<MealRViewModel> MealRs { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PropertyName { get; set; }
        public decimal? TotalPrice { get; set; }

    }
}
