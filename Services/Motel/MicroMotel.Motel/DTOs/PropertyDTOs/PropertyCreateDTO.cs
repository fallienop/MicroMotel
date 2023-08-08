using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.DTOs.PropertyDTOs.AddressDTOs;

namespace MicroMotel.Motel.DTOs.PropertyDTOs
{
    public class PropertyCreateDTO
    {
        public string Name { get; set; }
        public AddressDTO Address { get; set; }

        public byte RoomCount { get; set; }

      
        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpaces { get; set; }

    }
}
