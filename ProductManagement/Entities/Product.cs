using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalNote { get; set; }
        public int ProductCategoryId { get; set; }
        public int StatusId { get; set; }
        public bool Deleted { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductStatus Status { get; set; }
    }
}
