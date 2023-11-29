using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Models;

namespace FiorelloBackend.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<SliderVM>> GetAllAsync();
        Task<List<SliderVM>> GetAllWithTrueStatusAsync();
        Task<SliderVM> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task EditAsync(SliderEditVM slider);
    }
}
