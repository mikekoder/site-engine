using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("code")]
    public class CodeSnippetTagHelper : TagHelper
    {
        public string Lang { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "pre";
            var inner = output.GetChildContentAsync().Result;
            var code = inner.GetContent();
            output.Content.SetContent(code);
        }
    }
}
