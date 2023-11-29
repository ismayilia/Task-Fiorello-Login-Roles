using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorelloBackend.Areas.Admin.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [NotMapped]
        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
