using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("youtube")]
    public class YoutubeTagHelper : TagHelper
    {
        public string Id { get; set; }
        public int Width { get; set; } = 560;
        public int Height { get; set; } = 315;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "iframe";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.Attributes.Add("width", Width);
            output.Attributes.Add("height", Height);
            output.Attributes.Add("src", $"https://www.youtube.com/embed/{Id}");
            output.Attributes.Add("frameborder", 0);
            output.Attributes.Add("allow", "accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture");
            output.Attributes.Add("allowfullscreen", null);
        }
    }
}
