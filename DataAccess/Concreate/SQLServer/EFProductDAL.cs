using Core.DataAccess.EntityFremawork;
using Core.Helper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using Entities.Concreate;
using Entities.DTO.CartDTOs;
using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using static Entities.DTO.ProductDTO.ProductDTO;

namespace DataAccess.Concreate.SQLServer
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public async Task<IResult> AddProductByLanguage(ProductAddDTO productAddDTO, string userId)
        {

            try
            {
                using var context = new AppDbContext();

                List<Picture> pictureList = new();

                for (int i = 0; i < productAddDTO.PhotoUrls.Count; i++)
                {
                    pictureList.Add(new Picture { PhotoUrl = productAddDTO.PhotoUrls[i]});
                }

                Product product = new()
                {               
                    CategoryId = productAddDTO.CategoryId,
                    Quantity = productAddDTO.Quantity,
                    Price = productAddDTO.Price,
                    DisCount = productAddDTO.DisCount,
                    IsFeatured = productAddDTO.IsFeatured,
                    UserId = userId,
                    Pictures = pictureList,
                    Stock = false           
                };

                  await context.Products.AddAsync(product);
                  await context.SaveChangesAsync();

                for (int i = 0; i < productAddDTO.ProductNames.Count; i++)
                {
                    ProductLanguage productLanguage = new()
                    {
                        ProductId = product.Id,
                        ProductName = productAddDTO.ProductNames[i],
                        Description = productAddDTO.Descriptions[i],
                        LangCode = i == 0 ? "az-AZ" : i == 1 ? "ru-RU" : "en-EN",
                        SeoUrl = productAddDTO.ProductNames[i].ReplaceInvalidChars(),
                        
                    };
                 await context.ProductLanguages.AddAsync(productLanguage);
                }
                await context.SaveChangesAsync();

                return new SuccessResult(message: "Success Product Added");
            }
            catch (Exception ex)
            {
                return new ErrorResult(message: ex.Message);
            }
        }

        public async Task<bool> EditProductByLanguage(ProductDTO.ProductEditRecordDTO productEditRecordDTO)
        {

            using var context = new AppDbContext();

            List<Picture> pictures = new();

            for (int i = 0; i < productEditRecordDTO.PhotoUrls.Count; i++)
            {
                pictures.Add(new Picture { PhotoUrl = productEditRecordDTO.PhotoUrls[i] });
            }

            Product product = context.Products.FirstOrDefault(x => x.Id == productEditRecordDTO.Id);
            var picturesData = context.Pictures.Where(x => x.ProductId == productEditRecordDTO.Id).ToList();

            context.Pictures.RemoveRange(picturesData);
            await context.SaveChangesAsync();

            product.IsFeatured = productEditRecordDTO.IsFeatured;
            product.Price = productEditRecordDTO.Price;
            product.DisCount = productEditRecordDTO.DisCount;
            product.Quantity = productEditRecordDTO.Quantity;
            product.CategoryId = productEditRecordDTO.CategoryId;
            product.Pictures = pictures;

            context.Products.Update(product);

            var productLanguage = context.ProductLanguages.Where(x => x.ProductId == product.Id).ToList();

            for (int i = 0; i < productLanguage.Count; i++)
            {
                productLanguage[i].ProductName = productEditRecordDTO.ProductNames[i];
                productLanguage[i].Description = productEditRecordDTO.Descriptions[i];
                productLanguage[i].SeoUrl = productEditRecordDTO.ProductNames[i].ReplaceInvalidChars();
            }
            context.ProductLanguages.UpdateRange(productLanguage);
            await context.SaveChangesAsync();
            return true;

        }

        public int GetProductCountByCategory(double take, List<int> categoryIds)
        {
            using var context = new AppDbContext();
            var result = context.Products
                .Where(x => categoryIds.Any() == false ? null == null : categoryIds.Contains(x.CategoryId)).Count();
            return result;
        }

        public ProductDetailDTO GetProductDetail(int id, string langCode)
        {
            using AppDbContext context = new();

            var result = context.Products.Select(x => new ProductDetailDTO
            {
                Id = x.Id,
                ProductName = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).ProductName,
                Description = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Description,
                Price = x.Price,
                Discount = x.DisCount,
                Quantity = x.Quantity,
                PhotoUrls = x.Pictures.Where(x => x.ProductId == id).Select(x => x.PhotoUrl).ToList(),
            }).FirstOrDefault(x => x.Id == id);

            return result;
        }

        public List<ProductDetailFeaturedDTO> GetProductDetailFeaturedDTOs(string langCode, int id)
        {
            using var context = new AppDbContext();
          
            var result = context.ProductLanguages.Include(x => x.Product).ThenInclude(x =>x.Category)
                .Where(x => x.LangCode == langCode && x.Product.IsFeatured == true)
                .Select(x => new ProductDetailFeaturedDTO
                {
                    Id = x.Product.Id,
                    ProductName = x.ProductName,
                    Discount = x.Product.DisCount,
                    Price = x.Product.Price,
                    PhotoUrl = x.Product.Pictures.FirstOrDefault(y => y.ProductId == x.Product.Id).PhotoUrl
                }).ToList();

            return result;
        }

        public ProductEditRecordDTO GetProductEditDTO(int id)
        {

            using var context = new AppDbContext();

            var result = context.Products
                .Where(x => x.Id == id)
                .Select(x => new ProductEditRecordDTO(
                    x.Id,
                    x.Price,
                    x.DisCount,
                    x.Quantity,
                    x.Category.Id,
                    x.IsFeatured,
                    x.ProductLanguages.Select(y => y.ProductName).ToList(),
                    x.ProductLanguages.Select(y => y.Description).ToList(),
                    x.ProductLanguages.Select(y => y.SeoUrl).ToList(),
                    x.Pictures.Where(z => z.ProductId == x.Id).Select(x => x.PhotoUrl).ToList()
                    )).FirstOrDefault();

            return result;
        }

        public List<ProductFeaturedDTO> GetProductFeaturedDTOs(string langCode)
        {
            using var context = new AppDbContext();
            var result = context.ProductLanguages.Include(x => x.Product)
                .Where(x => x.LangCode == langCode && x.Product.IsFeatured == true) //--------sss-------//
                .Select(x => new ProductFeaturedDTO
                {
                    Id = x.Product.Id,
                    ProductName = x.ProductName,
                    Discount = x.Product.DisCount,
                    Price = x.Product.Price,
                    PhotoUrl = x.Product.Pictures.FirstOrDefault(y => y.ProductId == x.Product.Id).PhotoUrl
                }).ToList();

            return result;
        }

        public IEnumerable<ProductFilterDTO> GetProductFiltered(List<int> categoryIds, string langCode, int minPrice, int maxPrice, int pageNo, int take)
        {

            using AppDbContext context = new();

            int next = (pageNo - 1) * take;

            var result = context.Products
                .Include(x => x.ProductLanguages)
                .Include(x => x.Pictures)
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && (categoryIds.Any() ? categoryIds.Contains(x.CategoryId) : null == null))
                .Select(x => new ProductFilterDTO
                {
                    Id = x.Id,
                    Name = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).ProductName,
                    Price = x.Price,
                    Discount = x.DisCount,
                    PhotoUrl = x.Pictures.FirstOrDefault().PhotoUrl
                }).Skip(next).Take(take).ToList();

            return result;
        }

        public List<ProductRecentDTO> GetProductRecentDTOs(string langCode)
        {
            using var context = new AppDbContext();
            var result = context.ProductLanguages.Include(x => x.Product)
                .Where(x => x.LangCode == langCode && x.Product.IsFeatured == false)
                .Select(x => new ProductRecentDTO
                {
                    Id = x.Product.Id,
                    ProductName = x.ProductName,
                    Discount = x.Product.DisCount,
                    Price = x.Product.Price,
                    PhotoUrl = x.Product.Pictures.FirstOrDefault(y => y.ProductId == x.Product.Id).PhotoUrl
                }).ToList();

            return result;
        }

        public List<UserCartDTO> GetUserCartDTOs(List<int> Ids, string langCode)
        {

            using var context = new AppDbContext();

              var result = context.Products
                .Where(x => Ids.Contains(x.Id))
                .Select(x => new UserCartDTO
                {
                    Id = x.Id,
                    ProductName = x.ProductLanguages.FirstOrDefault(z => z.LangCode == langCode).ProductName,
                    PhotoUrl = x.Pictures.FirstOrDefault(z => z.ProductId == x.Id).PhotoUrl,
                    Price = x.DisCount != 0 ? x.DisCount : x.Price,
                    Quantity = x.Quantity
                }).ToList();
            return result;
        }

        public IDataResult<List<ProductAdminListDTO>> ProductAdminListDTOs(string userId, string langCode)
        {
            //using var context = new AppDbContext();
            //var result = context.Set<ProductAdminListDTO>().FromSqlInterpolated($"exec ProductAdminnListt @LangCode = {LangCode}").ToList();
            //return result;
            try
            {
                using AppDbContext context = new();

                var products = context.Products
                    .Include(x => x.ProductLanguages)
                    .Include(x => x.Pictures)
                    .Include(x => x.Category)
                    .ThenInclude(x => x.CategoryLanguages)
                    .Select(x => new ProductAdminListDTO
                    {
                        ProductName = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).ProductName,
                        Id = x.Id,
                        Price = x.Price,
                        //Email = "/",      
                        Discount = x.DisCount,
                        Quantity = x.Quantity,
                        CategoryName = x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName,
                        PhotoUrl = x.Pictures.FirstOrDefault().PhotoUrl                        
                    }).ToList();

                return new SuccessDataResult<List<ProductAdminListDTO>>(products, "Products were delivered successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ProductAdminListDTO>>(ex.Message);
            }
        }

        public IDataResult<List<ProductAdminListDTO>> ProductAdminListDTOs(string LangCode)
        {
            throw new NotImplementedException();
        }

        public ProductHomeDetail ProductHomeDetails(int id, string langCode)
        {


            using AppDbContext context = new();

            var result = context.Products.Include(x => x.ProductLanguages).Include(z => z.Pictures).Select(x => new ProductHomeDetail
            {
                Id = x.Id,
                ProductName = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).ProductName,
                //Description = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Description,
                Price = x.Price,
                Discount = x.DisCount,
                //Quantity = x.Quantity,
                PhotoUrl = x.Pictures.Where(x => x.ProductId == id).Select(x => x.PhotoUrl).FirstOrDefault(),
            }).FirstOrDefault();

            return result;
        }
    }
}
