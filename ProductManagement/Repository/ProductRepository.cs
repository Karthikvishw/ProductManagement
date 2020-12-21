using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Repository.Interfaces;
using Repository.Models;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        protected readonly ProductManagementContext _dbContext;

        public ProductRepository(ProductManagementContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<List<ProductModel>> ListAsync()
        {
            return await _dbContext.Products.Where(x => !x.Deleted).Select(x => new ProductModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Descriptions,
                AdditionalNote = x.AdditionalNote,
                Category = new ProductCategoryModel()
                            {
                                Id=x.ProductCategory.Id,
                                Name = x.ProductCategory.Name
                            },
                Status = new StatusModel()
                            {
                                Id = x.Status.Id,
                                Name = x.Status.Name
                            }
            }).ToListAsync();
        }

        public async Task<ReturnSearchItemModels<ProductModel>> SearchAsync(SearchItemModels searchItemModels)
        {
            return await (await ListAsync()).AsQueryable().SearchAsync(searchItemModels);
        }

        public async Task<ProductModel> SearchAsync(int productId)
        {
            return await _dbContext.Products.Where(x => !x.Deleted && x.Id == productId).Select( product => new ProductModel() {
                Id = product.Id,
                Name = product.Name,
                Description = product.Descriptions,
                AdditionalNote = product.AdditionalNote,
                Category = new ProductCategoryModel()
                {
                    Id = product.ProductCategory.Id,
                    Name = product.ProductCategory.Name
                },
                Status = new StatusModel()
                {
                    Id = product.Status.Id,
                    Name = product.Status.Name
                },
            }).SingleOrDefaultAsync() ?? throw new Exception("Product not found");

        }

        public async Task<ProductModel> AddorUpdateAsync(ProductModel product)
        {
            Products products = await _dbContext.Products.Where(x => !x.Deleted && x.Id == product.Id).SingleOrDefaultAsync() ?? new Products();
            if (products != null)
            {
                products.Name = product.Name;
                products.Descriptions = product.Description;
                products.AdditionalNote = product.AdditionalNote;
                products.CategoryId = product.Category.Id;
                products.StatusId = product.Status.Id;

                await _dbContext.SaveAsync();

                if (product.Id == 0)
                    product.Id = products.Id;
            } 
            return product;
        }
    }
}
