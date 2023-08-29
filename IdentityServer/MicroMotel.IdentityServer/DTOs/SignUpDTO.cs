using System.ComponentModel.DataAnnotations;

namespace MicroMotel.IdentityServer.DTOs
{
    public class SignUpDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string City { get; set; }

    }
}
