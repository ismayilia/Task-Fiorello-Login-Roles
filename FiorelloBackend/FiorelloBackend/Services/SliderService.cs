using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task DeleteAsync(int id)
        {
            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);


            _context.Sliders.Remove(dbSlider);
            await _context.SaveChangesAsync();

            string path = _env.GetFilePath("img", dbSlider.Img);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(SliderEditVM slider)
        {
            string oldPath = _env.GetFilePath("img", slider.Image);

            string fileName = $"{Guid.NewGuid()} - {slider.Photo.FileName}";

            string newPath = _env.GetFilePath("img", fileName);

            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == slider.Id);

            dbSlider.Img = fileName;

            await _context.SaveChangesAsync();



            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            await slider.Photo.SaveFile(newPath);
        }

        public async Task<List<SliderVM>> GetAllAsync()
        {

            return await _context.Sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Img, Status = m.Status })
                                         .ToListAsync();





            //List<SliderVM> models = new();
            //var datas = await _context.Sliders.ToListAsync();

            //foreach (var data in datas)
            //{
            //    models.Add(new SliderVM
            //    {
            //        Id = data.Id,
            //        Image = data.Img,
            //        Status = data.Status,
            //    });
            //}
            //return models;

        }

        public async Task<List<SliderVM>> GetAllWithTrueStatusAsync()
        {
            return await _context.Sliders.Where(m=>m.Status)
                                         .Select(m => new SliderVM { Id = m.Id, Image = m.Img, Status = m.Status })
                                         .ToListAsync();
        }

        public async Task<SliderVM> GetByIdAsync(int id)
        {
            return await _context.Sliders.Where(m => m.Id == id)
                                         .Select(m => new SliderVM { Id = m.Id, Image = m.Img, Status = m.Status })
                                         .FirstAsync();
        }
    }
}
