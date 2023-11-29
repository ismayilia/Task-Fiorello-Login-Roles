using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiorelloBackend.VIewComponenets
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILayoutIService _layoutIService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(ILayoutIService layoutIService, IHttpContextAccessor httpContextAccessor,
           UserManager<AppUser> userManager)
        {
            _layoutIService = layoutIService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            HeaderVM model = _layoutIService.GetHeaderDatas();

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is not null)
            {
                AppUser currentUser = await _userManager.FindByIdAsync(userId);
                model.UserFullName = currentUser.FullName;
            }
            
            
            return await Task.FromResult(View(model));
        }
    }
}
