namespace MicroMotel.Web.Models.Reservation.MealR
{
    public class MealRCreateInput
    {
        public int RoomRId { get; set; }

        public DateTime ReservationDate { get; set; }
        public int MealId { get; set; }
    }
}
