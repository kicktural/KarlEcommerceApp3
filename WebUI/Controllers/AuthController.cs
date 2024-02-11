using Entities.Concreate;
using Entities.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _env;
        public AuthController(UserManager<User> userManager, Microsoft.AspNetCore.Identity.SignInManager<User> signInResult, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInResult;
            _contextAccessor = contextAccessor;
            _env = env;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (checkEmail is null)
                {
                    ModelState.AddModelError("Error", "Email or Password is incorrect!");
                    return View();
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(checkEmail.Email, loginDTO.Password, loginDTO.Rememberme, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Email or Password is incorrect!");
                    return View();
                }
            }
            catch (Exception)
            {

                return View();
            }

        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {

                var EmailConfirm = await _userManager.FindByEmailAsync(registerDTO.Email);

                if (EmailConfirm is not null)
                {
                    ModelState.AddModelError("Error", "Registered with such an e-mail");
                    return View();
                }

                User UserRegister = new()
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email,
                    Address = "//",
                };

                var results = await _userManager.CreateAsync(UserRegister, registerDTO.Password);

                if (results.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var item in results.Errors)
                    {
                        ModelState.AddModelError("Error", item.Description);
                    }
                    return View();
                }
            }
            catch (Exception)
            {

                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
