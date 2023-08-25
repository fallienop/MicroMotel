using System.ComponentModel.DataAnnotations;

namespace MicroMotel.IdentityServer.DTOs
{
    public class UserUpdateDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public decimal? Budget { get; set; }

    }
}
