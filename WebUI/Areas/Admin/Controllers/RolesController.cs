using Core.Entities.Concreate.EntityFremawork;
using Entities.Concreate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.ViewModels;
using WebUI.Controllers;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area(nameof(Admin))]
    public class RolesController : Controller
    {

        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<AppRole> _roleManager;
        public RolesController(Microsoft.AspNetCore.Identity.UserManager<User> userManager, Microsoft.AspNetCore.Identity.RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(roles);
        }


      
        [Authorize]
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Role-Action")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {

            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorlist(result.Errors);
                return View();
            }
          

            return RedirectToAction(nameof(RolesController.Index));

        }


        [Authorize(Roles = "Role-Action")]
        [HttpGet]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("There is no role to be updated!");
            }

            return View(new RoleUpdateViewModel() { Id = roleToUpdate.Id, Name = roleToUpdate!.Name! });
        }



        [Authorize(Roles = "Role-Action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {

            var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);


            if (roleToUpdate == null)
            {
                throw new Exception("There is no role to be updated!");
            }

            roleToUpdate.Name = request.Name;

            await _roleManager.UpdateAsync(roleToUpdate);


            ViewData["SuccessMessage"] = "Role information has been updated.";

            return View();
        }



        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);

            if (roleToDelete == null)
            {
                ModelState.AddModelError(string.Empty, "The role to be deleted was not found!");
            }

            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }

            TempData["SuccessMessage"] = "Role deleted!";

            return RedirectToAction(nameof(RolesController.Index));
        }



        [HttpGet]
        public async Task<IActionResult> AssignRoleToUser(string id)
        {

            var currentUser = await _userManager.FindByIdAsync(id);

            ViewBag.userId = id;

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var roleViewModelList = new List<AssignRoleToUserViewModel>();


            foreach (var item in roles)
            {
                var assingRoleToUser = new AssignRoleToUserViewModel() { Id = item.Id, Name = item.Name! };


                if (userRoles.Contains(item.Name!)!)
                {
                    assingRoleToUser.Exsit = true;
                }

                roleViewModelList.Add(assingRoleToUser);
            }

            return View(roleViewModelList);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> requestList)
        {

			var userToAssignRoles = await _userManager.FindByIdAsync(userId);

			foreach (var role in requestList)
			{
				if (role.Exsit)
				{
					await _userManager.AddToRoleAsync(userToAssignRoles, role.Name);
				}
				else
				{
					await _userManager.RemoveFromRoleAsync(userToAssignRoles, role.Name);
				}
			}

			return RedirectToAction(nameof(UserListController.UserList), "UserList");
		}
    }
}
