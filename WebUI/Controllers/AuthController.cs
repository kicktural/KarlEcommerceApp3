using Entities.Concreate;
using Entities.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Extensions;
using WebUI.Helperservices;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
	public class AuthController : Controller
	{

		private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IWebHostEnvironment _env;
		private readonly IEmailService _emailService;

		public AuthController(Microsoft.AspNetCore.Identity.UserManager<User> userManager, Microsoft.AspNetCore.Identity.SignInManager<User> signInResult, IHttpContextAccessor contextAccessor, IWebHostEnvironment env, IEmailService emailService)
		{
			_userManager = userManager;
			_signInManager = signInResult;
			_contextAccessor = contextAccessor;
			_env = env;
			_emailService = emailService;
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

				}


				if (result.IsLockedOut)
				{
					ModelState.AddModelErrorlist(new List<string>() { "You cannot log in for 3 minutes." });
				}

				ModelState.AddModelErrorlist(new List<string>() {$"Email or password Incorrect",  $"(Number of failed logins = " +
				$"{await _userManager.GetAccessFailedCountAsync(checkEmail)})" });

				return View();

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
		public async Task<IActionResult> Register(RegisterDTO registerDTO, User user, string? password)
		{
			try
			{

				var EmailConfirm = await _userManager.FindByEmailAsync(registerDTO.Email);

				if (EmailConfirm is not null)
				{
					ModelState.AddModelError("Error", "Registered with such an e-mail");
					return View();
				}


				User userRegister = new()
				{
					FirstName = registerDTO.FirstName,
					LastName = registerDTO.LastName,
					Email = registerDTO.Email,
					UserName = registerDTO.Email,
				};

				var results = await _userManager.CreateAsync(userRegister, registerDTO.Password);

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


		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}




		public IActionResult ForgetPassword()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
		{
			

			var hasUser = await _userManager.FindByEmailAsync(request.Email);

			if (hasUser == null)
			{
				ModelState.AddModelError("Error", "No user found for this email address!");
				return View();
			}

			string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

			var passwordResetLink = Url.Action("ResetPassword", "Auth", new { userId = hasUser.Id, Token = passwordResetToken },
				HttpContext.Request.Scheme);


			await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

			TempData["success"] = "Password reset link has been sent to your email address";

			return RedirectToAction(nameof(ForgetPassword));
		}


		public IActionResult ResetPassword(string userId, string token)
		{
			TempData["userId"] = userId;
			TempData["token"] = token;

			return View();
		}


		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
		{
			var userId = TempData["userId"];
			var token = TempData["token"];

			if (userId == null || token == null)
			{
				throw new Exception("An error occurred!");
			}

			var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "No such user was found.");
				return View();
			}


			var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);


			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = "Your password has been successfully reset";
			}
			else
			{
				ModelState.AddModelErrorlist(result.Errors.Select(x => x.Description).ToList());
			}

			return View();
		}


		public IActionResult FacebookLogin(string ReturnUrl)
		{

			string RedirectUrl = Url.Action("ExternalResponse", "Auth", new { ReturnUrl = ReturnUrl });

			var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);

			return new ChallengeResult("Facebook", properties);
		}


		public IActionResult GoogleLogin(string ReturnUrl)
		{

			string RedirectUrl = Url.Action("ExternalResponse", "Auth", new { ReturnUrl = ReturnUrl });

			var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", RedirectUrl);

			return new ChallengeResult("Google", properties);
		}


		public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
		{
			
			try
			{
				ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

				if (info == null)
				{
					return RedirectToAction("Login");
				}
				else
				{
					Microsoft.AspNetCore.Identity.SignInResult result = await
						_signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);


					if (result.Succeeded)
					{
						return Redirect(ReturnUrl);
					}
					else
					{
						User user = new();

						user.Email = info.Principal.FindFirst(ClaimTypes.Email).Value;

						string ExternailUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;

						if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Name))
						{
							string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;

							userName = userName.Replace(' ', '-').ToLower() + ExternailUserId.Substring(0, 5).ToString();

							user.UserName = userName;
						}
						else
						{
							user.UserName = info.Principal.FindFirst(ClaimTypes.Email).Value;
						}


						IdentityResult createResult = await _userManager.CreateAsync(user);


						if (createResult.Succeeded)
						{
							IdentityResult loginResult = await _userManager.AddLoginAsync(user, info);


							if (loginResult.Succeeded)
							{
								await _signInManager.SignInAsync(user, isPersistent: true);
								return Redirect(ReturnUrl);
							}
							else
							{
								ModelState.AddModelError(string.Empty, "A problem occurred during AddLogin!");
							}
						}
						else
						{
							ModelState.AddModelError(string.Empty, "A problem occurred during create!");
						}


					}
				}


			}
			catch (Exception ex)
			{
				return View();
	
			}
	    		return Redirect("Error");



		}
	}

}