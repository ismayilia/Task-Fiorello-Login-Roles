using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _producService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IProductService productService, ICategoryService categoryService, IWebHostEnvironment env)
        {
            _context = context;
            _producService = productService;
            _categoryService = categoryService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int take = 4)
        {
            List<ProductVM> dbPaginatedDatas = await _producService.GetPaginatedDatasAsync(page, take);
            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductVM> paginatedDatas = new(dbPaginatedDatas, page, pageCount);

            return View(paginatedDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _producService.GetCountAsync();

            return (int)Math.Ceiling((decimal)(productCount) / take);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) BadRequest();

            Product product = await _producService.GetByIdWithIncludesAsync((int)id);

            if (product is null) NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return View(request);
            }


            foreach (var photo in request.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "File can be only image format");
                    return View(request);
                }

                if (!photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photos", "File size can  be max 100 kb");
                    return View(request);
                }
            }

            List<ProductImage> newImages = new();

            foreach (var photo in request.Photos)
            {
                string fileName = $"{Guid.NewGuid()} - {photo.FileName}";

                string path = _env.GetFilePath("img", fileName);

                await photo.SaveFile(path);

                newImages.Add(new ProductImage { Image = fileName });

            }

            newImages.FirstOrDefault().IsMain = true;

            await _context.ProductImages.AddRangeAsync(newImages);

            await _context.Products.AddAsync(new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Images = newImages
            });

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> GetCategoriesAsync()
        {
            return new SelectList(await _categoryService.GetAllAsync(), "Id", "Name");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) BadRequest();

            Product product = await _producService.GetByIdWithIncludesAsync((int)id);

            if (product is null) NotFound();


            await _producService.DeleteAsync((int)id);

            return RedirectToAction(nameof(Index));


        }


        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (id is null) BadRequest();

            Product product = await _producService.GetByIdWithIncludesAsync((int)id);

            if (product is null) NotFound();

            return View(new ProductEditVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = (int)product.CategoryId,
                Price = product.Price,
                Images = product.Images.ToList()

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int? id, ProductEditVM request)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (id is null) BadRequest();

            Product product = await _producService.GetByIdWithIncludesAsync((int)id);

            if (product is null) NotFound();

            request.Images = product.Images.ToList();

            if (!ModelState.IsValid)
            {
                return View(request); 
            }


            

            List<ProductImage> newImages = new();

            if (request.Photos != null)
            {
                foreach (var photo in request.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File can be only image format");
                        return View(request);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photos", "File size can  be max 100 kb");
                        return View(request);
                    }
                }
                foreach (var photo in request.Photos)
                {
                    string fileName = $"{Guid.NewGuid()} - {photo.FileName}";

                    string path = _env.GetFilePath("img", fileName);

                    await photo.SaveFile(path);

                    newImages.Add(new ProductImage { Image = fileName });

                }

                await _context.ProductImages.AddRangeAsync(newImages);
            }


            newImages.AddRange(request.Images);

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.Images = newImages;
            

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }





        [HttpPost]

        public async Task<IActionResult> DeleteProductImage(int id)
        {
            ProductImage image = await _context.ProductImages.FirstOrDefaultAsync(m => m.Id == id);


            _context.ProductImages.Remove(image);

            await _context.SaveChangesAsync();

            string path = _env.GetFilePath("img", image.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            return Ok();
        }
    }
}
