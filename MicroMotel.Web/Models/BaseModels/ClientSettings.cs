namespace MicroMotel.Web.Models.BaseModels
{
    public class ClientSettings
    {
        public Client Registered { get; set; } = null!;
        public Client UnRegistered { get; set; } = null!;
    }

    public class Client
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}
