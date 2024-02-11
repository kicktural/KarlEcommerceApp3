using Core.DataAccess.EntityFremawork;
using Core.Helper;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.Concreate;
using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Entities.DTO.CategoryDTOs.CategoryDTO;

namespace DataAccess.Concreate.SQLServer
{
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        public async Task<bool> AddCategory(CategoryAddDTO categoryAddDTO, IFormFile formFile, string WebRootPath)
        {
            try
            {
                using var context = new AppDbContext();
                Category category = new()
                {
                    PhotoUrl = await formFile.SaveFileAsync(WebRootPath),
                    IsFeatured = false
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                for (int i = 0; i < categoryAddDTO.CategoryName.Count; i++)
                {
                    CategoryLanguage categoryLanguage = new()
                    {
                        CategoryId = category.Id,
                        CategoryName = categoryAddDTO.CategoryName[i],
                        LangCode = categoryAddDTO.LangCode[i],

                    };
                    await context.CategoryLanguages.AddAsync(categoryLanguage);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public CategoryDetailDTO CategoryDetail(int id, string langCode)
        {
            using AppDbContext context = new();
            var result = context.Categories.Select(x => new CategoryDetailDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName,
                IsFeatured = x.IsFeatured,
                //PhotoUrl = x.Pictures.Where(x => x.ProductId == id).Select(x => x.PhotoUrl).ToList(),
                PhotoUrl = x.PhotoUrl
            }).FirstOrDefault(x => x.Id == id);

            return result;

        }

        public IEnumerable<CategoryFilterDTO> CategoryFilterDTOs(string langCode)
        {
            using AppDbContext context = new();
            var result = context.Categories
                .Include(x => x.CategoryLanguages)
                .Select(x => new CategoryFilterDTO
                {
                    Id = x.Id,
                    CategoryName = x.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName
                }).ToList();
            return result;
        }

        public List<CategoryNavbarDTO> CategoryNavbarDTOs(string langCode)
        {
            using var context = new AppDbContext();

            var result = context.CategoryLanguages.
               Where(x => x.LangCode == langCode)
               .Include(x => x.Category)
               .Select(x => new CategoryNavbarDTO(x.Category.Id, x.CategoryName))
               .ToList();
            return result;
        }

        public List<CategoryAdminListDTO> GetAllCategoresAdminList(string langCode)
        {
            using var context = new AppDbContext();

            var result = context.Categories.Select(x => new CategoryAdminListDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName,
                PhotoUrl = x.PhotoUrl,
                IsFeatured = x.IsFeatured
            }).ToList();

            return result;

        }

        public CategoryAdminDetailDTO GetCategoryByAdminDetail(int id)
        {
            using var context = new AppDbContext();

            var result = context.Categories.Include(x => x.CategoryLanguages)
                .Select(x => new CategoryAdminDetailDTO()
                {
                    Id = x.Id,
                    IsFeatured = x.IsFeatured,
                    PhotoUrl = x.PhotoUrl,
                    CategoryName = x.CategoryLanguages.Select(x => x.CategoryName).ToList(),
                })
                .FirstOrDefault(x =>x.Id == id);
            return result;
        }

        public List<CategoryFeaturedDTO> GetCategoryFeatured(string langCode)
        {
            using var context = new AppDbContext();

            var result = context.CategoryLanguages
                .Include(x => x.Category)
                .Where(x => x.LangCode == langCode && x.Category.IsFeatured == true)
                .Select(x => new CategoryFeaturedDTO(x.Category.Id, x.CategoryName, x.Category.PhotoUrl, x.Category.Products.Count)).ToList();

            return result;
        }

        public async Task<bool> UpdateCategory(CategoryAdminDetailDTO categoryAdminDetailDTO, IFormFile formFile, string webRootPath)
        {
            try
            {
                using var context = new AppDbContext();
                var category = context.Categories.FirstOrDefault(x => x.Id == categoryAdminDetailDTO.Id);

                category.IsFeatured = categoryAdminDetailDTO.IsFeatured;
                if (formFile != null)
                {
                    category.PhotoUrl = await formFile.SaveFileAsync(webRootPath);
                }

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                var categoryLanguage = context.CategoryLanguages.Where(x => x.CategoryId == category.Id).ToList();

                for (int i = 0; i < categoryLanguage.Count; i++)
                {
                    categoryLanguage[i].CategoryName = categoryAdminDetailDTO.CategoryName[i];
                }
                context.CategoryLanguages.UpdateRange(categoryLanguage);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
