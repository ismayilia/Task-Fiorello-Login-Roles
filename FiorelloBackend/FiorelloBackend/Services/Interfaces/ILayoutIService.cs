using FiorelloBackend.ViewModels.Home;

namespace FiorelloBackend.Services.Interfaces
{
    public interface ILayoutIService
    {
        HeaderVM GetHeaderDatas();
        FooterVM GetFooterDatas();
    }
}
