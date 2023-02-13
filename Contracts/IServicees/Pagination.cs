using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IServicees
{
    public class Pagination
    {
        const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        public int _pageSize { get; set; }


        public string SortBy { get; set; } = "Category";

        public string SortOrder { get; set; } = "ASC";
    }
}
