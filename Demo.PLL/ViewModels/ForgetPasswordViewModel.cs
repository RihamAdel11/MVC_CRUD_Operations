using System.ComponentModel.DataAnnotations;

namespace Demo.PLL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "InValid Email")]
        public string Email { get; set; }
    }
}
