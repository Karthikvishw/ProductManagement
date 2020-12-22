using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagement.Models;
using Repository.Interfaces;
using Repository.Models;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Privacy()
        {
            return await Task.Run(() => View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetProducts(SearchItemModels searchItem)
        {
            return Ok(await _productRepository.SearchAsync(searchItem));
        }

        public async Task<IActionResult> Edit(int productId)
        {
            var product = await _productRepository.SearchAsync(productId);

            return await Task.Run(() => View("Edit", product));
        }

        [HttpPost]
        public async Task Delete(int productId)
        {
            await _productRepository.DeleteAsync(productId);
        }

        [HttpPost]
        public async Task<IActionResult> AddorUpdate(ProductModel productModel)
        {
            return Ok(await _productRepository.AddorUpdateAsync(productModel));
        }
    }
}
