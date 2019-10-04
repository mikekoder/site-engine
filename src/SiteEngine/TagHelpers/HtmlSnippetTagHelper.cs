using HeyRed.MarkdownSharp;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("html-snippet")]
    public class HtmlSideBySideTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "row");
            var inner = output.GetChildContentAsync().Result;
            var content = inner.GetContent();
            var html = $@"<div class=""col s6"">{HttpUtility.HtmlEncode(content).Replace("\n", "<br />")}</div><div class=""col s6"">{content.Replace("\n","<br />")}</div>";
            output.Content.SetHtmlContent(html);
        }
    }
}
