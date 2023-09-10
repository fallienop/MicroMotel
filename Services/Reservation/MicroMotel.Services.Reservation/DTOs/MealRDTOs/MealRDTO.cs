namespace MicroMotel.Services.Reservation.DTOs.MealRDTOs
{
    public class MealRDTO
    {
        public int Id { get; set; }
        public int RoomRId { get; set; }

        public DateTime ReservationDate { get; set; }
        public int MealId { get; set; }

    }
}
