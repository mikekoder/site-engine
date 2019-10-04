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
    [HtmlTargetElement("collapsible")]
    public class CollapsibleTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.SetAttribute("class", "collapsible");

            var inner = output.GetChildContentAsync().Result;
            var content = inner.GetContent();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var html = "";
            foreach (var node in doc.DocumentNode.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Element))
            {
                var title = node.GetAttributeValue(":title", "");
                var icon = node.GetAttributeValue(":icon", "");
                node.Attributes.Remove(":title");
                node.Attributes.Remove(":icon");
                node.Name = "div";
                node.SetAttributeValue("class", "collapsible-body");
                html += "<li>";
                html += $@"<div class=""collapsible-header"">";
                if (!string.IsNullOrWhiteSpace(icon))
                {
                    html += $@"<i class=""material-icons"">{icon}</i>";
                }
                html += title;
                html += "</div>";
                html += node.OuterHtml;
                html += "</li>";
            }
            output.Content.SetHtmlContent(html);
        }
    }
}
