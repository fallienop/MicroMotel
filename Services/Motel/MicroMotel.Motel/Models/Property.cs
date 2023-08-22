using MicroMotel.Services.Motel.Models;
using System.ComponentModel.DataAnnotations;

    namespace MicroMotel.Motel.Models
    {
        public class Property
        {
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public Address Address { get; set; }

            public byte RoomCount { get; set; }
        
            public byte FloorCount { get; set; }
        
            public bool HasParking { get; set; }    
            public bool HasOpenSpace { get; set; } 
            public string Picture { get; set; }

            public ICollection<Room> Rooms { get; set; }    
            
            public ICollection<Meal> Meals { get; set; }
        } 
    }
