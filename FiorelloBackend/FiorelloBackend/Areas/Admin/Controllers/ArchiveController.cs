using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArchiveController : Controller
    {

        private readonly ICategoryService _categoryService;
        public ArchiveController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet]
        public async Task<IActionResult> Categories()
        {

            return View(await _categoryService.GetArchiveDatasAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> ExtractCategory(int id)
        {
            Category dbCategory = await _categoryService.GetSoftDeletedDataById(id);

            await _categoryService.ExtractAsync(dbCategory);
            return RedirectToAction(nameof(Categories));
        }
    }
}
