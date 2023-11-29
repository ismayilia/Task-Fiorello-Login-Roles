using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloBackend.VIewComponenets
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILayoutIService _layoutIService;

        public FooterViewComponent(ILayoutIService layoutIService)
        {
            _layoutIService = layoutIService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            FooterVM model = _layoutIService.GetFooterDatas();
            return await Task.FromResult(View(model));
        }
    }
}
