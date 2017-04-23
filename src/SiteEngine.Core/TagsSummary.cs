using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public class TagsSummary
    {
        public IEnumerable<TagSummary> Tags { get; set; }
    }
    public class TagSummary
    {
        public string Tag { get; set; }
        public int PageCount { get; set; }
    }
}
