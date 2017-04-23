using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("page-link", Attributes="page-id")]
    public class PageLinkTagHelper : TagHelper
    {
        public Page Page { get; set; }
        public string PageId { get; set; }

        private readonly Site site;
        public PageLinkTagHelper(Site site)
        {
            this.site = site;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var page = Page ?? site.Pages.SingleOrDefault(p => p.Id == PageId);
            if(page == null)
            {
                output.TagName = "span";
                output.Content.SetContent($"Sivua {PageId} ei löytynyt");
                return;
            }

            output.TagName = "a";
            output.Attributes.Add("href", page.Path);
            var inner = output.GetChildContentAsync().Result;
            if (inner.IsEmptyOrWhiteSpace)
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetContent(page.Title);
            }
            else
            {
                var text = inner.GetContent();
                output.Content.SetContent(text);
            }
        }
    }
}
