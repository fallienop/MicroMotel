using MicroMotel.Motel.Models;

namespace MicroMotel.Services.Motel.DTOs.PropertyDTOs
{
    public class PropertyWithRoomsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        public byte RoomCount { get; set; }

        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpace { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
