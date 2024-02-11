using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents
{
    public class CategoryNavbarViewComponent : ViewComponent
    {

        private readonly ICategoryService _categoryService;

        public CategoryNavbarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = _categoryService.GetAllCategorieNavbar("az-Az");
            return View("CategoryNavbar", result.Data);
        }
    }
}
