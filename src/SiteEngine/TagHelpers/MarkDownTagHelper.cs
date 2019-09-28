using HeyRed.MarkdownSharp;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Web;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("md")]
    public class MarkDownTagHelper : TagHelper
    {
        public bool TrimWhitespace { get; set; } = true;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var inner = output.GetChildContentAsync().Result;
            var content = inner.GetContent();
            var mark = new Markdown();
            if (TrimWhitespace)
            {
                var sb = new StringBuilder();
                int? trimLength = null;
                foreach (var line in content.Replace("\r\n", "\n").Split('\n'))
                {
                    if (!trimLength.HasValue)
                    {
                        var newLine = line.TrimStart();
                        if (newLine.Length > 0)
                        {
                            trimLength = line.Length - newLine.Length;
                        }
                    }
                    if (trimLength.HasValue && line.Length > trimLength.Value && string.IsNullOrWhiteSpace(line.Substring(0, trimLength.Value)))
                    {
                        sb.AppendLine(line.Substring(trimLength.Value));
                    }
                    else
                    {
                        sb.AppendLine(line.TrimStart());
                    }
                }
                content = sb.ToString();
                content = HttpUtility.HtmlDecode(content);
            }

            var formatted = Markdig.Markdown.ToHtml(content);// mark.Transform(content);

            output.Content.SetHtmlContent(formatted);
        }
    }
}
