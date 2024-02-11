using Business.Abstract;
using Core.Utilities.Results.Concreate;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;
using WebUI.ViewModels2;

namespace WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;

            var categoryView = _categoryService.GetAllCategoriesFeatured(currentColture);
            var categoryDetail = _categoryService.GetCategoryDetail(id, currentColture);
            DetailVM categoryDetailVM = new DetailVM()
            {
                CategoryDetail = categoryDetail.Data,
                CategoryFeaturedDTOs = categoryView.Data
            };

            if (categoryDetail.Success)
            {
                return View(categoryDetailVM);

            }
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
