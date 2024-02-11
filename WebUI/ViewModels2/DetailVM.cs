using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using static Entities.DTO.CategoryDTOs.CategoryDTO;

namespace WebUI.ViewModels2
{
    public class DetailVM
    {
        public ProductDetailDTO ProductDetail { get; set; }
        public CategoryDetailDTO CategoryDetail { get; set; }
        public List<ProductDetailFeaturedDTO> ProductDetailFeaturedDTO { get; set; }
        public List<CategoryFeaturedDTO> CategoryFeaturedDTOs { get; set; }
    }
}
