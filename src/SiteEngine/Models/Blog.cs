using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.Models
{
    public class Blog : Page
    {
        public string TagsDirectory { get; set; }
        public string CategoriesDirectory { get; set; }
        public string ArchivesDirectory { get; set; }

        public override object GetSubModel(string relativePath, Site site)
        {
            var segments = relativePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (!string.IsNullOrWhiteSpace(TagsDirectory) && segments[0].Equals(TagsDirectory, StringComparison.CurrentCultureIgnoreCase))
            {
                if (segments.Length == 1)
                {
                    return new TagIndexPage
                    {
                        Tags = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this).SelectMany(p => p.Tags).Distinct().ToArray()
                    };
                }
                else
                {
                    var tag = segments[1];
                    var children = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this && p.Tags.Contains(tag)).ToArray();
                    return new TagPage
                    {
                        Tag = tag,
                        Children = children
                    };
                }
            }
            else if (!string.IsNullOrWhiteSpace(CategoriesDirectory) && segments[0].Equals(CategoriesDirectory, StringComparison.CurrentCultureIgnoreCase))
            {
                if (segments.Length == 1)
                {
                    return new CategoryIndexPage
                    {
                        Categories = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this).SelectMany(p => p.Categories).Distinct().ToArray()
                    };
                }
                else
                {
                    var category = segments[1];
                    var children = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this && p.Categories.Contains(category)).ToArray();
                    return new CategoryPage
                    {
                        Category = category,
                        Children = children
                    };
                }
            }
            else if (!string.IsNullOrWhiteSpace(ArchivesDirectory) && segments[0].Equals(ArchivesDirectory, StringComparison.CurrentCultureIgnoreCase))
            {
                int year;
                int month;
                if(segments.Length > 1 && int.TryParse(segments[1],out year))
                {
                    var model = new ArchivePage
                    {
                        Year = year
                    };
                    if (segments.Length > 2 && int.TryParse(segments[2], out month))
                    {
                        model.Children = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this && p.Published.Year == year && p.Published.Month == month).ToArray();
                        model.Month = month;
                    }
                    else
                    {
                        model.Children = site.Pages.Where(p => p.Path.StartsWith(Path) && p != this && p.Published.Year == year).ToArray();
                    }

                    return model;
                }
            }
            else
            {

            }
            return null;
        }
        public class TagIndexPage
        {
            public string[] Tags { get; set; }
        }

        public class TagPage
        {
            public string Tag { get; set; }
            public Page[] Children { get; set; }
        }
        public class CategoryIndexPage
        {
            public string[] Categories { get; set; }
        }
        public class CategoryPage
        {
            public string Category { get; set; }
            public Page[] Children { get; set; }
        }
        public class ArchivePage
        {
            public int Year { get; set; }
            public int? Month { get; set; }
            public Page[] Children { get; set; }
        }
    }
}
