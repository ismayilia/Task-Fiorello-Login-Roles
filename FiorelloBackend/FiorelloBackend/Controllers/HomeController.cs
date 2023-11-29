using FiorelloBackend.Areas.Admin.ViewModels.Slider;
using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly ISliderService _sliderService;

        public HomeController(AppDbContext context, IProductService productService, IBasketService basketService, ISliderService sliderService)
        {
            _context = context;
            _productService = productService;
            _basketService = basketService;
            _sliderService = sliderService;


        }

        public async Task<IActionResult> Index() 
         {
            
            List<SliderVM> sliders = await _sliderService.GetAllWithTrueStatusAsync();
            SliderInfo sliderInfo =await _context.SliderInfos.FirstOrDefaultAsync();
            
            List<Product> products = await _productService.GetAllWithImagesByTakeAsync(12);
            List<Category> categories =await _context.Categories.Include(m=>m.Products).ToListAsync();
            HomeAbout homeAbout =await _context.HomeAbouts.FirstOrDefaultAsync();
            List<HomeAboutIcon> homeAboutIcons =await _context.HomeAboutIcons.Where(m=> !m.SoftDeleted).ToListAsync();
            List<Expert> experts =await _context.Experts.Where(m=> !m.SoftDeleted).ToListAsync();
            Subscribe subscribe =await _context.Subscribes.FirstOrDefaultAsync();
            List<Say> says =await _context.Says.Where(m => !m.SoftDeleted).ToListAsync();
            List<Instagram> instagrams =await _context.Instagrams.ToListAsync();


            HomeVM model = new HomeVM()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Products = products,
                Categories = categories,
                HomeAbout = homeAbout,
                HomeAboutIcons = homeAboutIcons,
                Experts = experts,
                Subscribe=subscribe,
                Says=says,
                Instagrams=instagrams
            };
            return View(model);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddBasket(int? id)
        {

            
            if (id is null) return BadRequest();

            Product product = await _productService.GetByIdAsync((int)id);

            if(product is null) return NotFound();

            _basketService.AddBasket((int)id,product);
           
            
            return Ok();
        }

        
              
                
    }
}