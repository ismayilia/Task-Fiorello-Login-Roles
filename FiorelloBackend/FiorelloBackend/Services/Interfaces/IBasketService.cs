using FiorelloBackend.Helpers.Responses;
using FiorelloBackend.Models;
using FiorelloBackend.ViewModels.Home;

namespace FiorelloBackend.Services.Interfaces
{
    public interface IBasketService
    {
        void AddBasket(int id, Product product);
        Task<List<BasketDetailVM>> GetBasketDatasAsync();
        int GetCount();
        Task<DeleteBasketResponse> DeleteItem(int id);
        Task<IconBasketPlusAndMinus> PlusIcon(int id);
        Task<IconBasketPlusAndMinus> MinusIcon(int id);


    }
}
