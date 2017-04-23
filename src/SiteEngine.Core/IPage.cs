using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine
{
    public interface IPage
    {
        string Id { get; set; }
        string Slug { get; set; }
        string StaticPath { get; set; }
        string ParentId { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        DateTime Published { get; set; }
        string[] Tags { get; set; }
        string[] Categories { get; set; }
        Page Parent { get; }
        string Path { get; }
        string ViewPath { get; }
        int? MenuPosition { get; set; }
        string Excerpt { get; set; }
        bool Draft { get; set; }
        int Order { get; set; }

        object GetSubModel(string relativePath, Site site);
    }
}
