namespace MicroMotel.Services.Reservation.DTOs.MealRDTOs
{
    public class MealRCreateDTO
    {
        public int RoomRId { get; set; }

        public DateTime ReservationDate { get; set; }
        public int MealId { get; set; }
    }
}
