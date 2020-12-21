using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ReturnSearchItemModels<ProductModel>> SearchAsync(SearchItemModels searchItemModels);

        Task<ProductModel> SearchAsync(int productId);

        Task<List<ProductModel>> ListAsync();

        Task<ProductModel> AddorUpdateAsync(ProductModel product);
    }
}
