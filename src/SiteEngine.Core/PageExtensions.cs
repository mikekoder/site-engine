using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public static class PageExtensions
    {
        public static string ArchivePath(this IArchiveContext context, int year, int? month)
        {
            if (context.Archives != null && !string.IsNullOrWhiteSpace(context.Archives.Directory))
            {
                if (month.HasValue)
                {
                    return $"{context.Path}{context.Archives.Directory}/{year}/{month:d2}/";
                }
                else
                {
                    return $"{context.Path}{context.Archives.Directory}/{year}/";
                }
            }
            else
            {
                if (month.HasValue)
                {
                    return $"{context.Path}{year}/{month:d2}/";
                }
                else
                {
                    return $"{context.Path}{year}/";
                }
            }
        }
        public static string TagPath(this ITagContext context, string tag)
        {
            if (context.Tagging != null && !string.IsNullOrWhiteSpace(context.Tagging.Directory))
            {

                return $"{context.Path}{context.Tagging.Directory}/{tag}/";
            }
            else
            {
                return $"{context.Path}{tag}/";
            }
        }
        public static string CategoryPath(this ICategoryContext context, string category)
        {
            if (context.Categorization != null && !string.IsNullOrWhiteSpace(context.Categorization.Directory))
            {

                return $"{context.Path}{context.Categorization.Directory}/{category}/";
            }
            else
            {
                return $"{context.Path}{category}/";
            }
        }

        public static bool TryResolveArchivePath(this IArchiveContext context, string relativePath, Site site, out object result)
        {
            var hasArchiveDir = context.Archives != null && !string.IsNullOrWhiteSpace(context.Archives.Directory);
            var parts = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if(hasArchiveDir && !parts[0].Equals(context.Archives.Directory, StringComparison.CurrentCultureIgnoreCase))
            {
                result = null;
                return false;
            }

            var yearIndex = hasArchiveDir ? 1 : 0;
            var monthIndex = hasArchiveDir ? 2 : 1;
            int year, month;

            if(parts.Length > yearIndex && int.TryParse(parts[yearIndex], out year))
            {
                var archivePage = new ArchivePage
                {
                    Year = year
                };

                if (parts.Length > monthIndex && int.TryParse(parts[monthIndex], out month))
                {
                    archivePage.Month = month;
                    archivePage.Results = site.Pages.ChildrenOf(context).Where(p => p.Published.Year == year && p.Published.Month == month).ToArray();
                }
                else
                {
                    archivePage.Results = site.Pages.ChildrenOf(context).Where(p => p.Published.Year == year).ToArray();
                }
                result = archivePage;
                return true;
            }
            else if(hasArchiveDir)
            {
                result = context.GetArchivesSummary(site);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
        public static bool TryResolveCategoryPath(this ICategoryContext context, string relativePath, Site site, out object result)
        {
            var hasCategoryDir = context.Categorization != null && !string.IsNullOrWhiteSpace(context.Categorization.Directory);
            var parts = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (hasCategoryDir && !parts[0].Equals(context.Categorization.Directory, StringComparison.CurrentCultureIgnoreCase))
            {
                result = null;
                return false;
            }

            var categoryIndex = hasCategoryDir ? 1 : 0;

            if (parts.Length > categoryIndex)
            {
                var category = parts[categoryIndex];

                result = new CategoryPage
                {
                    Category = category,
                    Results = site.Pages.ChildrenOf(context)
                    .Where(p => p.Categories != null && p.Categories.Any(c => c.Equals(category,StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray()
                };
                return true;
            }
            else
            {
                var children = site.Pages.ChildrenOf(context);
                var categories = children.Where(c => c.Categories != null).SelectMany(c => c.Categories).Distinct().ToArray();
                result = context.GetCategoriesSummary(site);
                return true;
            }
        }
        public static bool TryResolveTagPath(this ITagContext context, string relativePath, Site site, out object result)
        {
            var hasTagDir = context.Tagging != null && !string.IsNullOrWhiteSpace(context.Tagging.Directory);
            var parts = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (hasTagDir && !parts[0].Equals(context.Tagging.Directory, StringComparison.CurrentCultureIgnoreCase))
            {
                result = null;
                return false;
            }

            var tagIndex = hasTagDir ? 1 : 0;

            if (parts.Length > tagIndex)
            {
                var tag = parts[tagIndex];

                result = new TagPage
                {
                    Tag = tag,
                    Results = site.Pages.ChildrenOf(context)
                    .Where(p => p.Tags != null && p.Tags.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray()
                };
                return true;
            }
            else
            {
                result = context.GetTagsSummary(site);
                return true;
            }
        }

        public static ArchivesSummary GetArchivesSummary(this IArchiveContext context, Site site)
        {
            var children = site.Pages.ChildrenOf(context);
            return new ArchivesSummary
            {
                Archives = children.GroupBy(p => new { p.Published.Year, p.Published.Month }).Select(g => new ArchiveSummary
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PageCount = g.Count()
                }).OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).ToArray()
            };
        }
        public static CategoriesSummary GetCategoriesSummary(this ICategoryContext context, Site site)
        {
            var children = site.Pages.ChildrenOf(context);
            var categories = children.Where(c => c.Categories != null).SelectMany(c => c.Categories).Distinct().ToArray();
            return new CategoriesSummary
            {
                Categories = categories.Select(c => new CategorySummary
                {
                    Category = c,
                    PageCount = children.Where(p => p.Categories != null && p.Categories.Any(c2 => c2.Equals(c, StringComparison.CurrentCultureIgnoreCase))).Count()
                }).OrderBy(c => c.Category).ToArray()
            };
        }
        public static TagsSummary GetTagsSummary(this ITagContext context, Site site)
        {
            var children = site.Pages.ChildrenOf(context);
            var tags = children.Where(c => c.Tags != null).SelectMany(c => c.Tags).Distinct().ToArray();
            return new TagsSummary
            {
                Tags = tags.Select(tag => new TagSummary
                {
                    Tag = tag,
                    PageCount = children.Where(p => p.Tags != null && p.Tags.Any(t => t.Equals(tag, StringComparison.CurrentCultureIgnoreCase))).Count()
                }).OrderBy(t => t.Tag).ToArray()
            };
        }
    }
}
