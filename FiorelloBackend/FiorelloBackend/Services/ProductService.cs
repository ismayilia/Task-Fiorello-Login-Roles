using AutoMapper;
using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Data;
using FiorelloBackend.Helpers.Extentions;
using FiorelloBackend.Models;
using FiorelloBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloBackend.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;


        public ProductService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task DeleteAsync(int id)
        {
            Product dbProduct = await _context.Products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);


            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();

            foreach (var photo in dbProduct.Images)
            {
                string path = _env.GetFilePath("img", photo.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public async Task<List<ProductVM>> GetAllAsync()
        {
            return _mapper.Map<List<ProductVM>>(await _context.Products.Include(m=>m.Images).Include(m=> m.Category).ToListAsync());
        }

        public async Task<List<Product>> GetAllWithImagesByTakeAsync(int take)
        {
           return await _context.Products.Include(m => m.Images).Take(take).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task<Product> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Products
                           .Where(m => m.Id == id)
                           .Include(m => m.Images)
                           .Include(m => m.Category)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<ProductVM>> GetPaginatedDatasAsync(int page, int take)
        {
            List<Product> products = await _context.Products.Include(m => m.Images)
                                                            .Include(m => m.Category)
                                                            .Skip((page*take)-take)
                                                            .Take(take)
                                                            .ToListAsync();
            return _mapper.Map<List<ProductVM>>(products);
        }
    }
}
