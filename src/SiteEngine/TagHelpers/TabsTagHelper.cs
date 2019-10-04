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
    [HtmlTargetElement("tabs")]
    public class TabsTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            var inner = output.GetChildContentAsync().Result;
            var content = inner.GetContent();

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var tabSelectors = "";
            var tabContents = doc.DocumentNode.ChildNodes.Where(n => n.NodeType == HtmlNodeType.Element).ToArray();
            var tabSelectorWidth = 12 / tabContents.Length;
            foreach (var node in tabContents)
            {
                var title = node.GetAttributeValue(":title", "");
                node.Attributes.Remove(":title");
                if (string.IsNullOrWhiteSpace(node.Id))
                {
                    node.Id = Guid.NewGuid().ToString();
                }
                node.SetAttributeValue("class", "col s12");

                tabSelectors += $@"<li class=""tab col s{tabSelectorWidth}""><a href=""#{node.Id}"">{title}</a></li>";
            }
            var html = $@"
<div class=""row"">
  <div class=""col s12"">
    <ul class=""tabs"">
    {tabSelectors}
    </ul>
  </div>
{doc.DocumentNode.OuterHtml}
</div>";
            output.Content.SetHtmlContent(html);
        }
    }
}
