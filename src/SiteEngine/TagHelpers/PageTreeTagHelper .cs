using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("page-tree")]
    public class PageTreeTagHelper : TagHelper
    {
        public Page Page { get; set; }
        public string PageId { get; set; }
        public string ExcludeIds { get; set; }
        public int? Levels { get; set; }
        public Page Selected { get; set; }
        public string SelectedId { get; set; }
        public bool Self { get; set; }

        private readonly Site site;
        public PageTreeTagHelper(Site site)
        {
            this.site = site;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "collection");
            output.TagMode = TagMode.StartTagAndEndTag;

            Page = Page ?? (!string.IsNullOrWhiteSpace(PageId) ? site.Pages.SingleOrDefault(p => p.Id == PageId) : null);
            Selected = Selected ?? (!string.IsNullOrWhiteSpace(SelectedId) ? site.Pages.SingleOrDefault(p => p.Id == SelectedId) : null);

            if(Page == null)
            {
                output.Content.SetContent($"Sivua {PageId} ei löytynyt");
            }

            var buffer = new StringBuilder();
            if (Self)
            {
                Write(new[] { Page }, 0, buffer);
            }
            else
            {
                Write(site.Pages.ChildrenOf(Page).OrderBy(p => p.Order), 1, buffer);
            }

            output.Content.SetHtmlContent(buffer.ToString());
        }
        private void Write(IEnumerable<Page> pages, int level, StringBuilder buffer)
        {
            foreach (var page in pages)
            {
                buffer.AppendLine($@"<a class=""collection-item tree-item-{level}{(page == Selected ? " active": "")}"" href=""{page.Path}"">{page.Title}</a>");
                
                if (Levels == null || Levels.Value > level)
                {
                    var children = site.Pages.ChildrenOf(page).OrderBy(p => p.Order);
                    Write(children, level + 1, buffer);
                }
            }
        }
    }
}
