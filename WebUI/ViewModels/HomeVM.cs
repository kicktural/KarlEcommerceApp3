using Entities.Concreate;
using Entities.DTO.CartDTOs;
using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using static Entities.DTO.CategoryDTOs.CategoryDTO;

namespace WebUI.ViewModels
{
    public class HomeVM
    {
        public List<CategoryFeaturedDTO> CategoryFeatureds { get; set; }
        public List<ProductFeaturedDTO> ProductFeaturedDTOs { get; set; }
        public List<ProductRecentDTO> GetProductRecentDTOs { get; set; }
        public ProductHomeDetail ProductDetail { get; set; }
    }
}
