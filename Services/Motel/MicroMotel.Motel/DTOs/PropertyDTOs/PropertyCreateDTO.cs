using MicroMotel.Motel.Models;

namespace MicroMotel.Motel.DTOs.PropertyDTOs
{
    public class PropertyCreateDTO
    {
        public string Name { get; set; }
        public Address Address { get; set; }

        public byte RoomCount { get; set; }

      
        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpaces { get; set; }

    }
}
