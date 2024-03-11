using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.ViewModels
{
	public class UserEditViewModel
	{
        [Required]
		public string FirstName { get; set; }
      
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
