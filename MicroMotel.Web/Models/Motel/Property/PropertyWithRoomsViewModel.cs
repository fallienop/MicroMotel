using MicroMotel.Web.Models.Motel.Property.Address;
using MicroMotel.Web.Models.Motel.Room;

namespace MicroMotel.Web.Models.Motel.Property
{
    public class PropertyWithRoomsViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public decimal Price { get; set; }
        public byte BedCount { get; set; }
        public bool HasBath { get; set; }
        public bool HasFridge { get; set; }
        public bool HasTv { get; set; }
        public bool HasNetwork { get; set; }
        public bool HasAC { get; set; }
        public byte Floor { get; set; }
        public byte Status { get; set; }

        public int PropertyId { get; set; }
    }
}
