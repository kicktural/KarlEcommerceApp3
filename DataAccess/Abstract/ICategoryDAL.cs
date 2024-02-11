using Core.DataAccess;
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

namespace DataAccess.Abstract
{
    public interface ICategoryDAL : IRepositoryBase<Category>
    {
        Task<bool> AddCategory(CategoryAddDTO categoryAddDTO, IFormFile formFile, string WebRootPath);
        Task<bool> UpdateCategory(CategoryAdminDetailDTO categoryAdminDetailDTO,  IFormFile formFile, string webRootPath);
        List<CategoryAdminListDTO> GetAllCategoresAdminList(string LangCode);
        List<CategoryFeaturedDTO> GetCategoryFeatured(string langCode);
        IEnumerable<CategoryFilterDTO> CategoryFilterDTOs(string langCode);
        List<CategoryNavbarDTO> CategoryNavbarDTOs(string langCode);
        CategoryAdminDetailDTO GetCategoryByAdminDetail(int id);
        CategoryDetailDTO CategoryDetail(int id, string langCode);
    }
}
