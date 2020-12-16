using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public class ReturnSearchItemModels<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

    public class SearchItemModels
    {
        public int Skip { get; set; }

        public string SortDirection { get; set; } = "desc";
        public string OrderBy { get; set; }

        public int Take { get; set; }
        public string SeachTerm { get; set; }
    }
}
