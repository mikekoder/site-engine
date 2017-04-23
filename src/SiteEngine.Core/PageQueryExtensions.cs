using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public static class PageQueryExtensions
    {
        public static IEnumerable<Page> DescendantsOf(this IEnumerable<Page> pages, Page parent)
        {
            return pages.Where(p => p.Path.StartsWith(parent.Path) && p != parent);
        }
        public static IEnumerable<Page> ChildrenOf(this IEnumerable<Page> pages, Page parent)
        {
            return pages.Where(p => p.Parent == parent);
        }
    }
}
