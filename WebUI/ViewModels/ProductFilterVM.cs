using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;

namespace WebUI.ViewModels
{
    public class ProductFilterVM
    {
        public IEnumerable<ProductFilterDTO> ProductFilters { get; set; }
        public IEnumerable<CategoryFilterDTO> CategoryFilters { get; set; }
        public List<ProductDetailFeaturedDTO> ProductDetailFeatureds { get; set; }
    }
}
