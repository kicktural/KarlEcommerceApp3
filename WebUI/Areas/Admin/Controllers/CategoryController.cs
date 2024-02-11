using Business.Abstract;
using Core.Utilities.Results.Concreate;
using Entities.Concreate;
using Entities.DTO.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Entities.DTO.CategoryDTOs.CategoryDTO;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area(nameof(Admin))]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var resultData = _categoryService.GetAllCategoriesAdmin("en-EN");
            if (resultData.Success)
            {
                return View(resultData.Data);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryAddDTO categoryAddDTO, IFormFile Photo)
        {
           var result = await _categoryService.AddCategoryByLanguage(categoryAddDTO, Photo, _webHostEnvironment.WebRootPath);
            if (result.Success)
            {
                return Redirect("/Admin/category");
            }
            return View();

            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            var result = _categoryService.GetCategoryById(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryAdminDetailDTO categoryAdminDetailDTO, IFormFile Photo)
        {
           var result = await _categoryService.UpdateCategoryLanguageAsync(categoryAdminDetailDTO,  Photo, _webHostEnvironment.WebRootPath);
            if (result.Success)
            {

            return Redirect("/Admin/Category");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var result = _categoryService.GetCategoryById(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(CategoryAdminDetailDTO categoryAdminDetailDTO)
        {
            var result = _categoryService.RemoveCategory(categoryAdminDetailDTO.Id);
            if (result.Success)
            {
                return Redirect("/admin/category");
            }
            return View();
        }
    }
}
