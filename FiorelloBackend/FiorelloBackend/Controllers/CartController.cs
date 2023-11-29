using FiorelloBackend.Data;
using FiorelloBackend.Models;
using FiorelloBackend.Services;
using FiorelloBackend.Services.Interfaces;
using FiorelloBackend.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloBackend.Controllers
{
    public class CartController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;

        public CartController(IBasketService basketService, IProductService productService)
        {
            _basketService = basketService;
            _productService = productService;
        }
        public async Task<IActionResult> Index() => View (await _basketService.GetBasketDatasAsync());


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _basketService.DeleteItem(id);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> PlusIcon(int id) 
        {
            var data  = await _basketService.PlusIcon(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> MinusIcon(int id)
        {
            var data = await _basketService.MinusIcon(id);
            return Ok(data);
        }
    }
}