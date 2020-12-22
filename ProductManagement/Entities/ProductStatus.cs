using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
