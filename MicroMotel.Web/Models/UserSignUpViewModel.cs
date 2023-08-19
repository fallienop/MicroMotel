using System.ComponentModel.DataAnnotations;

namespace MicroMotel.Web.Models
{
    public class UserSignUpViewModel
    {
        [Display(Name ="Username")]
        [Required(ErrorMessage ="Username required")]
        public string UserName { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage ="Password required")]
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [Compare(nameof(Password),ErrorMessage ="Doesn't match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email required")]
        public string Email { get;set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "City required")]
        public string City { get; set; }

    }
}
