using FiorelloBackend.Areas.Admin.ViewModels.Product;
using FiorelloBackend.Models;

namespace FiorelloBackend.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllWithImagesByTakeAsync(int take);
        Task<Product> GetByIdWithIncludesAsync(int id);
        Task<List<ProductVM>> GetAllAsync();
        Task<List<ProductVM>> GetPaginatedDatasAsync(int page, int take);
        Task<int> GetCountAsync();
        Task DeleteAsync(int id);

    }
}
