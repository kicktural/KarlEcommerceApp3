using Entities.Concreate;
using Entities.DTO.CartDTOs;

namespace WebUI.ViewModels
{
    public class CheckoutVM
    {
        public User User { get; set; }
        public List<UserCartDTO> UserCartDTO { get; set; }
    }
}
