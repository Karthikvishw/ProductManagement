using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                Description = x.Description,
                AdditionalNote = x.AdditionalNote,
                CategoryId = x.ProductCategory.Id,
                Category = x.ProductCategory.Name,
                StatusId = x.Status.Id,
                Status = x.Status.Name
            }).ToListAsync();
        }

        public async Task<ReturnSearchItemModels<ProductModel>> SearchAsync(SearchItemModels searchItemModels)
        {
            var temp = await (await ListAsync()).AsQueryable().SearchAsync(searchItemModels);
            return temp;
        }

        public async Task<ProductModel> SearchAsync(int productId)
        {
            var product = await _dbContext.Products.Where(x => !x.Deleted && x.Id == productId).Select( product => new ProductModel() {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            AdditionalNote = product.AdditionalNote,
                            CategoryId = product.ProductCategory.Id,

                            Category = product.ProductCategory.Name,
                            StatusId = product.Status.Id,
                            Status = product.Status.Name
                        }).SingleOrDefaultAsync() ?? new ProductModel();

            product.Categories = await _dbContext.ProductCategories.Where(x => !x.Deleted).Select(category => new ProductCategoryModel()
            {
                Id = category.Id,
                Name = category.Name
            }).ToListAsync();

            product.ProductStatus = await _dbContext.ProductStatuses.Where(x => !x.Deleted).Select(status => new StatusModel()
            {
                Id = status.Id,
                Name = status.Name
            }).ToListAsync();

            return product;
        }

        public async Task<ProductModel> AddorUpdateAsync(ProductModel product)
        {
            Product products = await _dbContext.Products.Where(x => !x.Deleted && x.Id == product.Id).SingleOrDefaultAsync() ?? new Product();
            if (products != null)
            {
                products.Name = product.Name;
                products.Description = product.Description;
                products.AdditionalNote = product.AdditionalNote;
                products.ProductCategoryId = product.CategoryId;
                products.StatusId = product.StatusId;

                if (product.Id == 0)
                    _dbContext.Products.Add(products);

                await _dbContext.SaveAsync();

                if (product.Id == 0)
                    product.Id = products.Id;
            } 
            return product;
        }

        public async Task DeleteAsync(int productId)
        {
            Product product = await _dbContext.Products.Where(x => !x.Deleted && x.Id == productId).SingleOrDefaultAsync();

            _dbContext.Products.Remove(product);

            await _dbContext.SaveAsync();
        }
    }
}
