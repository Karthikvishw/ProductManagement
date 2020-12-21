using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalNote { get; set; }
        public ProductCategoryModel Category { get; set; }
        public StatusModel Status { get; set; }


        public List<ProductCategoryModel> Categories { get; set; }
        public List<StatusModel> ProductStatus { get; set; }
    }
}
