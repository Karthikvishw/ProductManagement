using System;
using System.Collections.Generic;

namespace Entities
{
    public partial class Status
    {
        public Status()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
