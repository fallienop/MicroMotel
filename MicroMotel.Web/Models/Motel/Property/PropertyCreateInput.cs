using MicroMotel.Web.Models.Motel.Property.Address;

namespace MicroMotel.Web.Models.Motel.Property
{
    public class PropertyCreateInput
    {
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }

        public byte RoomCount { get; set; }


        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpaces { get; set; }
        public string Picture { get; set; }

        public IFormFile PhotoFormFile { get; set; }
    }   
}
