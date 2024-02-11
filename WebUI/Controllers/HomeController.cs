using Business.Abstract;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;
using WebUI.Services;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private  LanguageService _languageService;
        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IProductService productService, LanguageService languageService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
            _languageService = languageService;
        }

        public IActionResult Index(int id)
        {
            ViewBag.NEWARRIVALS = _languageService.GetKey("NEW ARRIVALS").Value;
            ViewBag.RECENTARRIVALS = _languageService.GetKey("RECENT ARRIVALS").Value;
            ViewBag.FreeShopping = _languageService.GetKey("Free Shipping & Returns").Value;
            ViewBag.BUYNOW = _languageService.GetKey("BUY NOW").Value;
            ViewBag.Discount = _languageService.GetKey("20% Discount for all dresses").Value;
            ViewBag.Colorlib = _languageService.GetKey("USE CODE: Colorlib").Value;
            ViewBag.Students = _languageService.GetKey("20% Discount for students").Value;
            ViewBag.AddToCart = _languageService.GetKey("ADD TO CART").Value;
            ViewBag.TSHIRT = _languageService.GetKey("WHITE T-SHIRT").Value;
            ViewBag.FreeShoppingUntil = _languageService.GetKey("* Free shipping until 25 Dec 2017").Value;
            ViewBag.TESTIMONIALS = _languageService.GetKey("TESTIMONIALS").Value;
            ViewBag.Nunc = _languageService.GetKey("Nunc pulvinar molestie sem id blandit. Nunc venenatis interdum mollis. Aliquam finibus nulla quam, a iaculis justo finibus non. Suspendisse in fermentum nunc.Nunc pulvinar molestie sem id blandit. Nunc venenatis interdum mollis.").Value;
            ViewBag.Home = _languageService.GetKey("Home").Value;
            ViewBag.Pages = _languageService.GetKey("PAGES").Value;
            ViewBag.DRESESS = _languageService.GetKey("DRESSES").Value;
            ViewBag.SHOES = _languageService.GetKey("SHOES").Value;
            ViewBag.CONTACT = _languageService.GetKey("CONTACT").Value;
            ViewBag.ALL = _languageService.GetKey("ALL").Value;
            ViewBag.WOMAN = _languageService.GetKey("WOMAN").Value;
            ViewBag.MEN = _languageService.GetKey("MEN").Value;
            ViewBag.ACCESSORIES = _languageService.GetKey("ACCESSORIES").Value;
            ViewBag.KIDS = _languageService.GetKey("KIDS").Value;
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;

            var categories = _categoryService.GetAllCategoriesFeatured(currentColture);
            var products = _productService.GetProductFeaturedList(currentColture);
            var productDetail = _productService.ProductHomeDetail(id, "az-Az");
            var productRecent = _productService.GetProductRecentList(currentColture);
            HomeVM homeVM = new HomeVM()
            {
                CategoryFeatureds = categories.Data,
                ProductFeaturedDTOs = products.Data,
                ProductDetail = productDetail.Data,
                GetProductRecentDTOs = productRecent.Data
            };       
            return View(homeVM);
        }


        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
