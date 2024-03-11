using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required]
        [MinLength(6)]
        public string PasswordOld { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required]
        [MinLength(6)]
        public string PasswordNew { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required]
        [MinLength(6)] 
        [Compare(nameof(PasswordNew), ErrorMessage = "Sifre ayni deyildir!")]     
        public string PasswordNewConfirm { get; set; } = null!;
    }
}
