using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.ViewModels
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role name can not be left empty!")]
        public string Name { get; set; }
    }
}
