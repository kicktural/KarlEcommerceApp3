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
			var currentColture = Thread.CurrentThread.CurrentCulture.Name;
			var result = _categoryService.GetAllCategorieNavbar(currentColture);
            return View("CategoryNavbar", result.Data);
        }
    }
}
