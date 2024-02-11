using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using DataAccess.Concreate.SQLServer;
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

//business usage: is there such a thing or not?

namespace Business.Concreate
{
    public class CategoryManager : ICategoryService
    { 
        private readonly ICategoryDAL _categoryDAL;

        public CategoryManager(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }

        public async Task<IResult> AddCategoryByLanguage(CategoryAddDTO categoryAddDTO, IFormFile formFile, string WebRootPath)
        {
            try
            {
               var result = await _categoryDAL.AddCategory(categoryAddDTO, formFile, WebRootPath);
                return new SuccessResult("SuccessFully completed");
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<CategoryNavbarDTO>> GetAllCategorieNavbar(string langCode)
        {
            try
            {
                var result = _categoryDAL.CategoryNavbarDTOs(langCode);
                return new SuccessDataResult<List<CategoryNavbarDTO>>(result);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<CategoryNavbarDTO>>(ex.Message);

            }
        }

        public IDataResult<int> GetAllCategories()
        {
            int result = _categoryDAL.GetAll().Count;
            return new SuccessDataResult<int>(result);
        }

        public IDataResult<List<CategoryAdminListDTO>> GetAllCategoriesAdmin(string langCode)
        {          
            try
            {
                var result = _categoryDAL.GetAllCategoresAdminList(langCode);
                return new SuccessDataResult<List<CategoryAdminListDTO>>(result, "Success! Data");
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<CategoryAdminListDTO>>(ex.Message);
            }
        }

        public IDataResult<List<CategoryFeaturedDTO>> GetAllCategoriesFeatured(string langCode)
        {
            try
            {
                var resultFeatured = _categoryDAL.GetCategoryFeatured(langCode);
                return new SuccessDataResult<List<CategoryFeaturedDTO>>(resultFeatured, "Success! Data");
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<CategoryFeaturedDTO>>(ex.Message);
            }
        }

        public IDataResult<IEnumerable<CategoryFilterDTO>> GetAllFilterCategories(string langCode)
        {
            var result = _categoryDAL.CategoryFilterDTOs(langCode);
            return new SuccessDataResult<IEnumerable<CategoryFilterDTO>>(result);
        }

        public IDataResult<CategoryAdminDetailDTO> GetCategoryById(int id)
        {
            try
            {
            var result = _categoryDAL.GetCategoryByAdminDetail(id);
            return new SuccessDataResult<CategoryAdminDetailDTO>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<CategoryAdminDetailDTO>(ex.Message);
            }
        }

        public IDataResult<CategoryDetailDTO> GetCategoryDetail(int id, string langCode)
        {
            try
            {

            var result = _categoryDAL.CategoryDetail(id, langCode);
            return new SuccessDataResult<CategoryDetailDTO>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<CategoryDetailDTO>(ex.Message);
            }
        }

        public IResult RemoveCategory(int id)
        {
            var category = _categoryDAL.Get(x => x.Id == id);
            _categoryDAL.Delete(category);
            return new SuccessResult("Deleted Successfully");
        }

        public async Task<IResult> UpdateCategoryLanguageAsync(CategoryAdminDetailDTO categoryAdminDetailDTO, IFormFile formFile, string webRootPath)
        {
            var result = await _categoryDAL.UpdateCategory(categoryAdminDetailDTO, formFile, webRootPath);
            if (result)
            {
                return new SuccessResult("Success");
            }
            return new ErrorResult("Error");
        }
    }
}
