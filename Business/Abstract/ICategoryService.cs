using Core.Utilities.Results.Abstract;
using Entities.Concreate;
using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.DTO.CategoryDTOs.CategoryDTO;

namespace Business.Abstract
{
    //business usage: is there such a thing or not?
    public interface ICategoryService
    {
        Task<IResult> AddCategoryByLanguage(CategoryAddDTO categoryAddDTO, IFormFile formFile, string WebRootPath); 
        Task<IResult> UpdateCategoryLanguageAsync(CategoryAdminDetailDTO categoryAdminDetailDTO,  IFormFile formFile, string webRootPath);
        IResult RemoveCategory(int id);
        IDataResult<List<CategoryAdminListDTO>> GetAllCategoriesAdmin(string langCode);
        IDataResult<List<CategoryFeaturedDTO>> GetAllCategoriesFeatured(string langCode);
        IDataResult<List<CategoryNavbarDTO>> GetAllCategorieNavbar(string langCode);
        IDataResult<CategoryAdminDetailDTO> GetCategoryById(int id);
        IDataResult<CategoryDetailDTO> GetCategoryDetail(int id, string langCode);
        IDataResult<IEnumerable<CategoryFilterDTO>> GetAllFilterCategories(string langCode);
        IDataResult<int> GetAllCategories();
    } 
}
