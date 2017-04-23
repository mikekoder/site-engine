using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public class CategoriesSummary
    {
        public IEnumerable<CategorySummary> Categories { get; set; }
    }
    public class CategorySummary
    {
        public string Category { get; set; }
        public int PageCount { get; set; }
    }
}
