namespace MicroMotel.Web.Models.BaseModels
{
    public class ServiceURLs
    {
        public string IdentityServerURL { get; set; }
        
        public string PhotoStockURL { get; set; }
        
        public string GatewayURL { get; set; }
        
        public ServiceAPI Motel { get; set; }
        
        public ServiceAPI Reservation { get; set; }
        public ServiceAPI Photos { get; set; }

    }

    public class ServiceAPI
    {
        public string Path { get; set; }

    }
}
