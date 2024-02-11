using Business.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Entities.DTO.CartDTOs;
using Entities.DTO.OrderDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        public readonly IProductService _productService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private readonly IOrderServices _orderServices;
        public CartController(IProductService productService, IHttpContextAccessor contextAccessor, IUserService userService, IOrderServices orderServices)
        {
            _productService = productService;
            _contextAccessor = contextAccessor;
            _userService = userService;
            _orderServices = orderServices;
        }

        public JsonResult AddToCart(int id, int quantity)
        {
            var cartCookie = Request.Cookies["cart"];
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(20);
            cookieOptions.Secure = true;
            cookieOptions.Path = "/";

            List<CartCookieDTO> cartCookies = new();
            CartCookieDTO cartCookieDTO = new()
            {
                Id = id,
                Quantity = quantity,
            };

            if (cartCookie == null)
            {
                cartCookies.Add(cartCookieDTO);
                var cookieJson = JsonSerializer.Serialize<List<CartCookieDTO>>(cartCookies);
                Response.Cookies.Append("cart", cookieJson, cookieOptions);
            }
            else
            {
                var data = JsonSerializer.Deserialize<List<CartCookieDTO>>(cartCookie);
                var findData = data.FirstOrDefault(x => x.Id == id);
                if (findData != null)
                {
                    findData.Quantity += 1;
                }
                else
                {
                    data.Add(cartCookieDTO);
                }
                var cookieJson = JsonSerializer.Serialize<List<CartCookieDTO>>(data);
                Response.Cookies.Append("cart", cookieJson, cookieOptions);
            }

            return Json("");

        }

        public IActionResult UserCart()
        {
            return View();
        }

        public JsonResult GetProduct()
        {
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;
            var cartCookie = Request.Cookies["cart"];
            var data = JsonSerializer.Deserialize<List<CartCookieDTO>>(cartCookie);

            var quantity = data.Select(x => x.Quantity).ToList();
            var dataIds = data.Select(x => x.Id).ToList();

            var result = _productService.GetProductForCart(dataIds, langCode: currentColture, quantities: quantity);
            return Json(result.Data);
        }


       
        public JsonResult RemoveData(string id)
        {
            var cookie = Request.Cookies["cart"];
            var cookieOptionsRemove = new CookieOptions();

            var data = JsonSerializer.Deserialize<List<CartCookieDTO>>(cookie);
            var result = data.FirstOrDefault(x => x.Id == Convert.ToInt32(id));

            data.Remove(result);

            var cookieJson = JsonSerializer.Serialize(data);
            Response.Cookies.Append("cart", cookieJson, cookieOptionsRemove);

            return Json("ok");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var currentColture = Thread.CurrentThread.CurrentCulture.Name;

            var cookie = Request.Cookies["cart"];
            var data = JsonSerializer.Deserialize<List<CartCookieDTO>>(cookie);
            var quantity = data.Select(x => x.Quantity).ToList();
            var dataIds = data.Select(x => x.Id).ToList();
            var result = _productService.GetProductForCart(dataIds, langCode: currentColture, quantity);

            CheckoutVM checkoutVM = new()
            {
                UserCartDTO = result.Data,
                User = user.Data
            };
            return View(checkoutVM);



        }

        [HttpPost]
        public IActionResult Checkout(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var currentColture = Thread.CurrentThread.CurrentCulture.Name;

            var cookie = Request.Cookies["cart"];
            var data = JsonSerializer.Deserialize<List<CartCookieDTO>>(cookie);
            var quantity = data.Select(x => x.Quantity).ToList();
            var dataIds = data.Select(x => x.Id).ToList();
            var result = _productService.GetProductForCart(dataIds, langCode: currentColture, quantity);

            List<OrderCreateDTO> orderList = new();

            foreach (var item in result.Data)
            {
                OrderCreateDTO orderCreateDTO = new()
                {
                    UserId = userId,
                    ProductId = item.Id,
                    ProductPrice = item.Price,
                    ProductQuantity = item.Quantity,
                    Message = "Null"
                };
                orderList.Add(orderCreateDTO);
            }

            // Ürün miktarını kontrol et
            var quantityCheckResult = _orderServices.CheckProducyQuantity(orderList);
            if (quantityCheckResult is ErrorResult)
            {
                TempData["ErrorMessage"] = "Ürünlerin bazıları stokta yok.";
                return RedirectToAction("Checkout", "Cart");
            }



            // Ürün miktar sınırlarını kontrol et
            var quantityLimitCheckResult = _orderServices.CheckProductQuantityLimit(orderList);
            if (quantityLimitCheckResult is ErrorResult)
            {
                TempData["ErrorMessage"] = "Ürünlerin bazıları için miktar sınırını aştınız.";

                return RedirectToAction("Checkout", "Cart");
            }


            _orderServices.CreateOrder(orderList);

            return RedirectToAction("Index", "Home");

        }
     
       
    }
}
