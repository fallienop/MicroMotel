using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.DTOs.PropertyDTOs.AddressDTOs;

namespace MicroMotel.Motel.DTOs.PropertyDTOs
{
    public class PropertyUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDTO Address { get; set; }

        public byte RoomCount { get; set; }

        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpace { get; set; }
    }
}
