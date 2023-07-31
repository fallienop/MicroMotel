namespace MicroMotel.Services.Reservation.Models
{
    public class MealR
    {
        public int Id { get; set; }
        public int RoomRId { get;set; }
     
        public DateTime ReservationDate { get; set; }   
        public int MealId { get; set; }

    }
}
