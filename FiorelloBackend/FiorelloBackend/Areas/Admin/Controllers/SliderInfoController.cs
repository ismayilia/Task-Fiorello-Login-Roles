using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderInfoController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            SliderInfo sliderInfo = await _context.SliderInfos.Where(m => !m.SoftDeleted).FirstOrDefaultAsync();
            return View(sliderInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo is null) return NotFound();

            return View(sliderInfo);


        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SliderInfo sliderInfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!sliderInfo.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File can be only image format");
                return View();
            }

            if (!sliderInfo.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "File size can  be max 100 kb");
                return View();
            }


            string fileName = $"{Guid.NewGuid()} - {sliderInfo.Photo.FileName}";

            sliderInfo.Img = fileName;

            string path = _env.GetFilePath("img", fileName);

            await _context.SliderInfos.AddAsync(sliderInfo);
            await _context.SaveChangesAsync();

            await sliderInfo.Photo.SaveFile(path);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            SliderInfo dbSlider = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);




            _context.SliderInfos.Remove(dbSlider);
            await _context.SaveChangesAsync();

            string path = _env.GetFilePath("img", dbSlider.Img);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo is null) return NotFound();

            return View(sliderInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int? id, SliderInfo sliderInfo)
        {

            if (id is null) return BadRequest();
           

            SliderInfo dbSliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (dbSliderInfo is null) return NotFound();

            sliderInfo.Img = dbSliderInfo.Img;

            ModelState.Remove("Photo");

            if (!ModelState.IsValid)
            {
                return View(sliderInfo);
            }

            if (sliderInfo.Photo is null)
            {
                dbSliderInfo.Title = sliderInfo.Title;
                dbSliderInfo.Description = sliderInfo.Description;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!sliderInfo.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File can be only image format");
                return View(sliderInfo);
            }

            if (!sliderInfo.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "File size can  be max 200 kb");
                return View(sliderInfo);
            }


            string oldPath = _env.GetFilePath("img", dbSliderInfo.Img);

            string fileName = $"{Guid.NewGuid()} - {sliderInfo.Photo.FileName}";

            string newPath = _env.GetFilePath("img", fileName);


            
            dbSliderInfo.Img = fileName;
            dbSliderInfo.Title = sliderInfo.Title;
            dbSliderInfo.Description = sliderInfo.Description;

            await _context.SaveChangesAsync();



            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            await sliderInfo.Photo.SaveFile(newPath);

            return RedirectToAction(nameof(Index));
        }

    }
}
