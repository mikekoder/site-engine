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

        private readonly Site site;
        public PageTreeTagHelper(Site site)
        {
            this.site = site;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;

            if(Page == null && !string.IsNullOrWhiteSpace(PageId))
            {
                Page = site.Pages.SingleOrDefault(p => p.Id == PageId);
            }
            if(Page == null)
            {
                output.Content.SetContent($"Sivua {PageId} ei löytynyt");
            }

            var buffer = new StringBuilder();
            Write(Page, 1, buffer);
            
            output.Content.SetHtmlContent(buffer.ToString());
        }
        private void Write(Page parent, int level, StringBuilder buffer)
        {
            foreach (var page in site.Pages.ChildrenOf(parent))
            {
                buffer.AppendLine($@"<li><a href=""{page.Path}"">{page.Title}</a></li>");
                if (Levels == null || Levels.Value > level)
                {
                    buffer.AppendLine("<ul>");
                    Write(page, level + 1, buffer);
                    buffer.AppendLine("</ul>");
                }
            }
        }
    }
}
