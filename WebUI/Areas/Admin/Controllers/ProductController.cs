using Business.Abstract;
using Entities.DTO.CategoryDTOs;
using Entities.DTO.ProductDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static Entities.DTO.ProductDTO.ProductDTO;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area(nameof(Admin))]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor, ICategoryService categoryService)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = _productService.GetAllProductAdminList(userId, "az-Az");
            if (result.Success)
            {
                return View(result.Data);
            }
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllCategoriesFeatured("az-AZ");
            ViewBag.Categories = new SelectList(categories.Data, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductAddDTO productAddDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _productService.AddProductByLanguageAsync(productAddDTO, userId);
            if (result.Success)
                return RedirectToAction("Index");

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _productService.GetProductEdit(id);
            var categories = _categoryService.GetAllCategoriesFeatured("az-AZ");
            ViewBag.Categories = new SelectList(categories.Data, "Id", "CategoryName");
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditRecordDTO productEditRecordDTO)
        {
            var result = await _productService.EditProductByLanguageAsync(productEditRecordDTO);
            if (result.Success)
                return RedirectToAction("Index");

            return View(result);
        }

        [HttpPost]
        public IActionResult Remove(int id, ProductEditRecordDTO product)
        {
            var result = _productService.RemoveProduct(product.Id);
            if (result.Success)
            {
                return Redirect("/admin/product");
            }
            return View();
        }
    }
}
