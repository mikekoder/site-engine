using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public static class PageQueryExtensions
    {
        public static IEnumerable<Page> DescendantsOf(this IEnumerable<Page> pages, IPage parent)
        {
            return pages.Where(p => p.Path.StartsWith(parent.Path) && p != parent);
        }
        public static IEnumerable<Page> ChildrenOf(this IEnumerable<Page> pages, IPage parent)
        {
            return pages.Where(p => p.Parent == parent);
        }
        public static IEnumerable<Page> BreadcrumpTo(this IEnumerable<Page> pages, IPage page)
        {
            var segments = page.Path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var paths = new[] { "/" };
            if (segments.Length >= 1)
            {
                paths = paths.Union(Enumerable.Range(1, segments.Length).Select(i => "/" + string.Join("/", segments.Take(i)) + "/")).ToArray();
            }
            return pages.Where(p => paths.Contains(p.Path)).OrderBy(p => p.Path.Length);
        }
        public static IEnumerable<Page> AncestorsOf(this IEnumerable<Page> pages, IPage page)
        {
            if(page.Parent == null)
            {
                return Enumerable.Empty<Page>();
            }
            return pages.BreadcrumpTo(page.Parent);
        }
    }
}
