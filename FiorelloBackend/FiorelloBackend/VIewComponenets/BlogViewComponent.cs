using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.VIewComponenets
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        

        public BlogViewComponent(AppDbContext context)
        {
            _context = context;
            

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();

            return await Task.FromResult(View(blogs));
        }
    }
}
