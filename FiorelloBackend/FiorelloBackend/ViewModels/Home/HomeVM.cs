using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Models;

namespace FiorelloBackend.ViewModels.Home
{
    public class HomeVM
    {
        public List<SliderVM> Sliders { get; set; }
        public SliderInfo SliderInfo { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public HomeAbout HomeAbout { get; set; }
        public List<HomeAboutIcon> HomeAboutIcons { get; set; }
        public List<Expert> Experts { get; set; }
        public Subscribe Subscribe { get; set; }
        public List<Say> Says { get; set; }
        public List<Instagram> Instagrams { get; set; }
    }
}
