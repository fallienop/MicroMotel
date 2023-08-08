using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MicroMotel.Web.Models.BaseModels
{
    public class SigninInput
    {
        [Required]
        [Display(Name = "Your username")]
        public string UserName { get; set; }
       
        
        [Display(Name = "Your password")]
        public string Password { get; set; }


        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; }

    }
}
