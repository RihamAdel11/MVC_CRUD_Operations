using System.ComponentModel.DataAnnotations;

namespace Demo.PLL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = " New Pasward Is Required")]
        [DataType(DataType.Password)]
        //[MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Dosnot Match")]
        public string ConfirmPassword { get; set;}
    }
}
