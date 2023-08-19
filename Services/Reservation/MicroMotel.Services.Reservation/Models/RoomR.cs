
using System.ComponentModel.DataAnnotations;

namespace MicroMotel.Services.Reservation.Models
{
    public class RoomR
    {
        [Key]
        public int Id { get;set; }
        public int PropertyId { get; set; } 
        public int RoomId { get; set; }  
        public DateTime ReservStart { get; set; }    
        public DateTime ReservEnd { get; set;}
        public ICollection<MealR>? MealRs { get; set;}
        public string UserID { get; set; }


    }
}
