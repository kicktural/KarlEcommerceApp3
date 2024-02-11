using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concreate;
using Entities.DTO.CartDTOs;
using Entities.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.DTO.ProductDTO.ProductDTO;

namespace DataAccess.Abstract
{
    public interface IProductDAL : IRepositoryBase<Product>
    {
        Task<IResult> AddProductByLanguage(ProductAddDTO productAddDTO, string userId);
        IDataResult<List<ProductAdminListDTO>> ProductAdminListDTOs(string userId, string lanCode);
        Task<bool> EditProductByLanguage(ProductEditRecordDTO productEditRecordDTO);
        ProductEditRecordDTO GetProductEditDTO(int id);
        List<ProductFeaturedDTO> GetProductFeaturedDTOs(string langCode);
        List<ProductRecentDTO> GetProductRecentDTOs(string langCode);
        IEnumerable<ProductFilterDTO> GetProductFiltered(List<int> categoryIds, string langCode, int minPrice, int maxPrice, int pageNo, int take);
        List<ProductDetailFeaturedDTO> GetProductDetailFeaturedDTOs(string langCode, int id);
        int GetProductCountByCategory(double take, List<int> categoryIds);
        List<UserCartDTO> GetUserCartDTOs(List<int> Ids, string langCode);
        ProductHomeDetail ProductHomeDetails(int id, string langCode);
        ProductDetailDTO GetProductDetail(int id, string langCode);
    }
}
