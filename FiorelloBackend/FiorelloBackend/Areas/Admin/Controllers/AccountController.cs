using FiorelloBackend.Areas.Admin.ViewModels.Account;
using FiorelloBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //userin adi familyasi email usernam ve rol
        // UserVM

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            

            List<UserVM> userVM = new();
            foreach (var user in users)
            {
				var roles = await _userManager.GetRolesAsync(user);

				userVM.Add(new UserVM
                {
                    FullName = user.FullName,
                    Username = user.UserName,
                    Email = user.Email,
                    RoleName = roles
                });
            }

            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoleToUser()
        {
            ViewBag.roles = await GetRolesAsync();
            ViewBag.users = await GetUsersAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleToUser(UserRoleVM request)
        {
            AppUser user = await _userManager.FindByIdAsync(request.UserId);
            IdentityRole role = await _roleManager.FindByIdAsync(request.RoleId);

            await _userManager.AddToRoleAsync(user, role.Name);

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> GetRolesAsync()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

            return new SelectList(roles, "Id", "Name");
        }

        private async Task<SelectList> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return new SelectList(users, "Id", "UserName");
        }
    }
}
