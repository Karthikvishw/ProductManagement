using System;
namespace Entities
{
    public partial class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string AdditionalNote { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public bool Deleted { get; set; }

        public virtual ProductCategories ProductCategory{ get; set; }
        public virtual Status Status { get; set; }
    }
}
