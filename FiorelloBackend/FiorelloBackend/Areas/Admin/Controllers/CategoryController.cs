using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers;
using FiorelloBackend.Helpers.Enums;
using FiorelloBackend.Models;
using FiorelloBackend.Services;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoryController(AppDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {

            List<Category> dbPaginatedDatas = await _categoryService.GetPaginatedDatasAsync(page, take);
            int pageCount = await GetPageCountAsync(take);

            Paginate<Category> paginatedDatas = new(dbPaginatedDatas, page, pageCount);

            return View(paginatedDatas);
        }
        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _categoryService.GetCountAsync();

            return (int)Math.Ceiling((decimal)(productCount) / take);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            category.Name = category.Name.Replace("\r\n", "");

            Category existCategory = await _categoryService.GetByNameAsync(category.Name);
            if (existCategory is not null)
            {
                ModelState.AddModelError("Name", "This name is already exists!");
                return View();
            }

            await _categoryService.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            Category dbCategory = await _categoryService.GetByIdAsync(id,false);

            await _categoryService.DeleteAsync(dbCategory);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>SoftDelete(int id)
        {
            Category dbCategory = await _categoryService.GetByIdAsync(id, true);

            await _categoryService.SoftDeleteAsync(dbCategory);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit (int? id)
        {
            if (id is null) return BadRequest();


            Category category = await _categoryService.GetByIdWithoutTracking((int)id);

            if (category is null) return NotFound();

            return View(category);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (id is null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            Category dbCategory = await _categoryService.GetByIdAsync((int)id,false);

            if (dbCategory is null) return NotFound();

            if (category.Name == dbCategory.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            Category existCategory = await _categoryService.GetByNameAsync(category.Name);
            if (existCategory is not null)
            {
                ModelState.AddModelError("Name", "This name is already exists!");
                return View(category);
            }

            //dbCategory.Name = category.Name;
            await _categoryService.EditAsync(category); 

            return RedirectToAction(nameof(Index));
        }

    }



}
