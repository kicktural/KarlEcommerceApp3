using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using Entities.DTO.CartDTOs;
using Entities.DTO.ProductDTO;
using static Entities.DTO.ProductDTO.ProductDTO;

namespace Business.Concreate
{
    public class ProductManager : IProductService
    {
        private readonly IProductDAL _productDAL;

        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public async Task<IResult> AddProductByLanguageAsync(ProductAddDTO productAddDTO, string userId)
        {
            var result = await _productDAL.AddProductByLanguage(productAddDTO, userId);
            if (result.Success)
                return new SuccessResult(message: "Success! Added Product");

            return new ErrorResult(result.Message);
        }

        public async Task<IResult> EditProductByLanguageAsync(ProductEditRecordDTO productEditRecordDTO)
        {
            try
            {

                var result = await _productDAL.EditProductByLanguage(productEditRecordDTO);
                return new SuccessResult(message: "SuccessFully Edit!");
            }
            catch (Exception ex)
            {

                return new ErrorResult(message: ex.Message);
            }
        }

        public IDataResult<IEnumerable<ProductFilterDTO>> GetAllFilteredProducts(List<int> categoryIds, string langCode, int minPrice, int maxPrice, int pageNo, int take)
        {
            try
            {

                var result = _productDAL.GetProductFiltered(categoryIds, langCode, minPrice, maxPrice, pageNo, take);
                return new SuccessDataResult<IEnumerable<ProductFilterDTO>>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<IEnumerable<ProductFilterDTO>>(ex.Message);
            }
        }

        public IDataResult<List<ProductAdminListDTO>> GetAllProductAdminList(string userId, string langCode)
        {
            //var result = _productDAL.ProductAdminListDTOs(langCode);
            //return new SuccessDataResult<List<ProductAdminListDTO>>(result.Data);
            var result = _productDAL.ProductAdminListDTOs(userId, langCode);
            if (result.Success)
            {
                return new SuccessDataResult<List<ProductAdminListDTO>>(result.Data);
            }
            return new ErrorDataResult<List<ProductAdminListDTO>>(result.Message);
        }

        public IDataResult<ProductDetailDTO> GetProductBYId(int id, string langCode)
        {
            var result = _productDAL.GetProductDetail(id, langCode);
            return new SuccessDataResult<ProductDetailDTO>(result);
        }

        public IDataResult<int> GetProductByIdQuantity(int productId)
        {
            var result = _productDAL.Get(x => x.Id == productId).Quantity;
            return new SuccessDataResult<int>(result);
        }

        public IDataResult<int> GetProductCount(double take, List<int> categoryIds)
        {
            double res = _productDAL.GetProductCountByCategory(take, categoryIds) / take;
            int productCountResult = (int)Math.Ceiling((double)res);

            return new SuccessDataResult<int>(productCountResult);
        }

        public IDataResult<List<ProductDetailFeaturedDTO>> GetProductDetailFeaturedList(string langCode, int id)
        {
            try
            {

                var result = _productDAL.GetProductDetailFeaturedDTOs(langCode, id);
                return new SuccessDataResult<List<ProductDetailFeaturedDTO>>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<ProductDetailFeaturedDTO>>(message: ex.Message);
            }
        }

        public IDataResult<ProductDTO.ProductEditRecordDTO> GetProductEdit(int id)
        {
            try
            {

                var result = _productDAL.GetProductEditDTO(id);
                return new SuccessDataResult<ProductEditRecordDTO>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<ProductEditRecordDTO>(ex.Message);
            }
        }

        public IDataResult<List<ProductFeaturedDTO>> GetProductFeaturedList(string langCode)
        {
            try
            {
                var result = _productDAL.GetProductFeaturedDTOs(langCode);
                return new SuccessDataResult<List<ProductFeaturedDTO>>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<ProductFeaturedDTO>>(ex.Message);
            }

        }

        public IDataResult<List<UserCartDTO>> GetProductForCart(List<int> ids, string langCode, List<int> quantities)
        {
            var result = _productDAL.GetUserCartDTOs(ids, langCode);

            for (int i = 0; i < result.Count; i++)
            {
                result[index: i].Quantity = quantities[index: i];
            }
            return new SuccessDataResult<List<UserCartDTO>>(result);
        }

        public IDataResult<List<ProductRecentDTO>> GetProductRecentList(string langCode)
        {
            try
            {
                var result = _productDAL.GetProductRecentDTOs(langCode);
                return new SuccessDataResult<List<ProductRecentDTO>>(result);

            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<ProductRecentDTO>>(ex.Message);
            }
        }

        public IDataResult<ProductHomeDetail> ProductHomeDetail(int id, string langCode)
        {
            var result = _productDAL.ProductHomeDetails(id, langCode);
            return new SuccessDataResult<ProductHomeDetail>(result);
        }

        public IResult RemoveProduct(int id)
        {
            var product = _productDAL.Get(x => x.Id == id);
            _productDAL.Delete(product);
            return new SuccessResult("Deleted Successfully");
        }
    }
}
