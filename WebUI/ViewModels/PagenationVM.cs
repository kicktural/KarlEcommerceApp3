using Entities.DTO.ProductDTO;

namespace WebUI.ViewModels
{
    public class PagenationVM
    {
        public int ProductCount { get; set; }
        public IEnumerable<ProductFilterDTO> Products { get; set; }
    }
}
