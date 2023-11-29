using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels.Home;

namespace FiorelloBackend.Services
{
    public class LayoutService : ILayoutIService
    {
        private readonly IBasketService _basketService;
        private readonly ISettingService _settingService;

        public LayoutService(IBasketService basketService, ISettingService settingService)
        {
            _basketService = basketService;
            _settingService = settingService;

        }

        public HeaderVM GetHeaderDatas()
        {
            Dictionary<string,string> settingDatas = _settingService.GetSettings();
            int basketCount = _basketService.GetCount();
            return new HeaderVM
            {
                BasketCount = basketCount,
                Logo = settingDatas["HeaderLogo"]
            };

        }

        public FooterVM GetFooterDatas()
        {
            Dictionary<string, string> settingDatas = _settingService.GetSettings();
            
            return new FooterVM
            {
                PhoneNumber = settingDatas["Phone"],
                Address = settingDatas["Address"]
            };

        }
    }
}
