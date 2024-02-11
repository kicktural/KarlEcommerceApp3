using Core.Utilities.Results.Abstract;
using Entities.DTO.CartDTOs;
using Entities.DTO.ProductDTO;
using static Entities.DTO.ProductDTO.ProductDTO;

namespace Business.Abstract
{
    public interface IProductService
    {
        Task<IResult> AddProductByLanguageAsync(ProductAddDTO productAddDTO, string userId);
        Task<IResult> EditProductByLanguageAsync(ProductEditRecordDTO productEditRecordDTO);
        IDataResult<List<ProductAdminListDTO>> GetAllProductAdminList(string userId, string langCode);
        IDataResult<ProductEditRecordDTO> GetProductEdit(int id);
        IResult RemoveProduct(int id);
        IDataResult<List<ProductFeaturedDTO>> GetProductFeaturedList(string langCode);
        IDataResult<List<ProductRecentDTO>> GetProductRecentList(string langCode);
        IDataResult<List<ProductDetailFeaturedDTO>> GetProductDetailFeaturedList(string langCode, int id);
        IDataResult<List<UserCartDTO>> GetProductForCart(List<int> ids, string langCode, List<int> quantities);
        IDataResult<int> GetProductByIdQuantity(int productId);
        IDataResult<ProductHomeDetail> ProductHomeDetail(int id, string langCode);
        IDataResult<ProductDetailDTO> GetProductBYId(int id, string langCode);
        IDataResult<int> GetProductCount(double take, List<int> categoryIds);
        IDataResult<IEnumerable<ProductFilterDTO>> GetAllFilteredProducts(List<int> categoryIds, string langCode, int minPrice, int maxPrice, int pageNo, int take);

    }
}
