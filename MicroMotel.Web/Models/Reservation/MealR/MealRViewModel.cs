namespace MicroMotel.Web.Models.Reservation.MealR
{
    public class MealRViewModel
    {
        public int Id { get; set; }
        public int RoomRId { get; set; }

        public DateTime ReservationDate { get; set; }
        public int MealId { get; set; }
        public string MealName { get; set; }    
    }
}
