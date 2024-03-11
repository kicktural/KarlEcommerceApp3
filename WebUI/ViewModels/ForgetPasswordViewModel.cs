using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress(ErrorMessage = "Email formati yalnistir!")]
		public string Email { get; set; }
	}
}
