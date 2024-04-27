using System.ComponentModel.DataAnnotations;

namespace Demo.PLL.ViewModels
{
	public class RegisterViewModel
	{
        [Required(ErrorMessage = "FirstName Is Required")]
        public string Fname { get; set; }

		[Required(ErrorMessage = "LastName Is Required")]
		public string Lname { get; set; }

		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress (ErrorMessage ="InValid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password Is Required")]
		[DataType (DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage = "Password Dosnot Match")]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
