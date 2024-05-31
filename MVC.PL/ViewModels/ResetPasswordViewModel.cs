using System.ComponentModel.DataAnnotations;

namespace MVC.PL.ViewModels
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
