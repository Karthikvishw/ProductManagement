using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public class ReturnSearchItemModels<T>
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<T> data { get; set; }
    }

    public class SearchItemModels
    {
        public int Start { get; set; }

        public string SortDirection { get; set; } = "desc";
        public string OrderBy { get; set; }

        public int Length { get; set; }
        public string SeachTerm { get; set; }
    }
}
