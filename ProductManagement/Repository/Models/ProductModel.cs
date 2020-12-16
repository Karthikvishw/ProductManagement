using System;
namespace Repository.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string AdditionalNote { get; set; }
        public ProductCategoryModel Category { get; set; }
        public StatusModel Status { get; set; }
    }
}
