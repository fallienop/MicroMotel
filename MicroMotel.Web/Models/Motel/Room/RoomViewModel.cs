namespace MicroMotel.Web.Models.Motel.Room
{
    public class RoomViewModel
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

        public string StatusText
        {
            get {
                switch (Status)
                {
                      case 1:return "Available";
                      case 2:return "In Use";
                      case 3:return "Unavailable";
                      case 4: return "Problematic";
                      default:return "unsigned statement";
                        

                }
            }
        }

        public string Color
        {
            get
            {
                switch (Status)
                {
                    case 1: return "Green";
                    case 2: return "Blue";
                    case 3: return "Orange";
                    case 4: return "Red";
                    default: return "White";


                }
            }
        }
    }
}
