using Entities.Concreate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebUI.Areas.ViewModels;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IFileProvider _fileProvider;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel
            {
                Email = currentUser!.Email,
                UserName = currentUser.UserName,
                PictureUrl = currentUser.Picture,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
            };
            return View(userViewModel);
        }


        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);


            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Your old password is wrong!");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorlist(result.Errors);
                return View();
            }


            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Your password has been successfully changed";


            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {


            var currentuser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userEditViewModel = new UserEditViewModel()
            {
                FirstName = currentuser.FirstName,
                LastName = currentuser.LastName,
                Email = currentuser.Email
            };

            return View(userEditViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }


            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser.FirstName = request.FirstName;
            currentUser.LastName = request.LastName;
            currentUser.Email = request.Email;

            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension
                    (request.Picture.FileName)}";


                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userPictures")
                    .PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }

            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorlist(updateToUserResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, isPersistent: true);

            TempData["SuccessMessage"] = "Your information has been successfully changed";



            var userEditViewModel = new UserEditViewModel()
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email
            };

            return View(userEditViewModel);
        }

    }
}
