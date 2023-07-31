using MicroMotel.Motel.Models;

namespace MicroMotel.Motel.DTOs.PropertyDTOs
{
    public class PropertyUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        public byte RoomCount { get; set; }

        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpace { get; set; }
    }
}
