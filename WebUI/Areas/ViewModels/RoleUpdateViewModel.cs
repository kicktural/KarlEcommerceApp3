using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.ViewModels
{
    public class RoleUpdateViewModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Role name can not be left empty!")]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;
    }
}
