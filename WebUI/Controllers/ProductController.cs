using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services;
using WebUI.ViewModels;
using WebUI.ViewModels2;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private LanguageService _languageService;
        public ProductController(IProductService productService, ICategoryService categoryService, LanguageService languageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _languageService = languageService;
        }

        public IActionResult Index(int id, List<int> categoryIds, int page = 1)        
        {
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;
            ViewBag.CurrentPage = page;
            ViewBag.ProductCount = _productService.GetProductCount(3, categoryIds).Data;

            var products = _productService.GetAllFilteredProducts(categoryIds, currentColture, 0, maxPrice: 10000, pageNo: page, take: 3);
            var categories = _categoryService.GetAllFilterCategories(currentColture);
            var productFeatured = _productService.GetProductDetailFeaturedList(id: id, langCode: currentColture);
            ProductFilterVM productFilterVM = new()
            {
                ProductFilters = products.Data,
                CategoryFilters = categories.Data,
                ProductDetailFeatureds = productFeatured.Data,

            };
            return View(productFilterVM);
        }

         

        public IActionResult Detail(int id)
        {
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;
            ViewBag.RecentProducts = _languageService.GetKey("RECENT IS FEATURED PRODUCTS").Value;
            var productDetail = _productService.GetProductBYId(id, currentColture);
            var productFeatured = _productService.GetProductDetailFeaturedList(id: id, langCode: currentColture);
            //var result3 = _categoryService.GetAllCategoriesFeatured("az-Az");
            DetailVM detailVM = new()
            {
                ProductDetail = productDetail.Data,
                ProductDetailFeaturedDTO = productFeatured.Data,
                //CategoryFeaturedDTOs = result3.Data
            };

            if (productDetail.Success)
            {
                return View(detailVM);
                
            }
            return RedirectToAction(nameof(Index), "Home");
        }



        public JsonResult GetDatas(int page, int take, string categoryList, int minPrice, int maxPrice)
        {
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;
            var categories = _categoryService.GetAllFilterCategories(langCode: currentColture);
            var cats = new List<int>();

            if (categoryList == "null")
            {
                cats = categories.Data.Select(x => x.Id).ToList();
            }
            else
            {
                cats = categoryList.Split(",").Select(x => Convert.ToInt32(x)).ToList();
            }

            var result = _productService.GetAllFilteredProducts(cats, langCode: currentColture, 0, maxPrice: 10000, pageNo: page, take: take);
            var productCount = _productService.GetProductCount(take, cats).Data;
            PagenationVM paginationVM = new()
            {
                ProductCount = productCount,
                Products = result.Data
            };
            return Json(paginationVM);
        }
    }
}
