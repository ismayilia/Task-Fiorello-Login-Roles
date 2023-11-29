using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FiorelloBackend.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SliderController : Controller
	{

        private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;

        public SliderController(AppDbContext context, IWebHostEnvironment env, ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
		{
			 
			return View(await _sliderService.GetAllAsync());
		}

		[HttpGet]
		public async Task<IActionResult> Detail(int? id)
		{
			if (id is null) return BadRequest();

			SliderVM slider = await _sliderService.GetByIdAsync((int) id);

			if (slider is null) return NotFound();

			return View(slider);
			
			
		}

        [HttpGet]

		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SliderCreateVM slider)
		{
			if (!ModelState.IsValid)
			{
				return View();
			} 

			foreach (var photo in slider.Photos)
			{
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "File can be only image format");
                    return View();
                }

                if (!photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photos", "File size can  be max 100 kb");
                    return View();
                }
            }


			foreach (var photo in slider.Photos)
			{
                string fileName = $"{Guid.NewGuid()} - {photo.FileName}";

                string path = _env.GetFilePath("img", fileName);


				Slider newSlider = new()
				{
					Img = fileName
				};

                await _context.Sliders.AddAsync(newSlider);
                await _context.SaveChangesAsync();



                await photo.SaveFile(path);
            }
			

			


				return RedirectToAction(nameof(Index));
		}


        [HttpGet]
		public async Task<IActionResult> Edit(int? id) 
		{
            if (id is null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

			SliderEditVM model = new()
			{
				Image = slider.Img
			};

            return View(model);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Edit(int? id, SliderEditVM slider)
		{

            if (id is null) return BadRequest();

            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (dbSlider is null) return NotFound();

			slider.Image = dbSlider.Img;


				
            if(slider.Photo is null)
            {
               return RedirectToAction(nameof(Index));
            }

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File can be only image format");
                return View(slider);
            }

            if (!slider.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "File size can  be max 200 kb");
                return View(slider);
            }

            await _sliderService.EditAsync(slider);

            return RedirectToAction(nameof(Index));
		}


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {

			await _sliderService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangeStatus(int id)
		{

			int count = await _context.Sliders.Where(m => m.Status).CountAsync();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

			if (slider.Status)
			{
				if (count != 1)
				{
					slider.Status = false;
				}
				else
				{
                    return RedirectToAction(nameof(Index));
                }
                
            }
			else
			{
				slider.Status = true;
			}


			//slider.Status = !slider.Status;
			await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
		}
    }
}
