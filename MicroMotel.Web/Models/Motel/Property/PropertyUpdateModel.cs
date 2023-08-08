using MicroMotel.Web.Models.Motel.Property.Address;

namespace MicroMotel.Web.Models.Motel.Property
{
    public class PropertyUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }

        public byte RoomCount { get; set; }

        public byte FloorCount { get; set; }

        public bool HasParking { get; set; }
        public bool HasOpenSpace { get; set; }
    }
}
