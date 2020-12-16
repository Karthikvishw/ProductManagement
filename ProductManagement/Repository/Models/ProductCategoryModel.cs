
using System;
namespace Repository.Models
{
    public class ProductCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator ProductCategoryModel(ProductModel v)
        {
            throw new NotImplementedException();
        }
    }
}
