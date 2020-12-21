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

        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productRepository.ListAsync());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.SearchAsync(id);

            return await Task.Run(() => View("Edit", product));
        }

        public async Task<IActionResult> AddorUpdate([FromBody] ProductModel productModel)
        {
            return Ok(await _productRepository.AddorUpdateAsync(productModel));
        }
    }
}
