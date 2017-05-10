using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteEngine.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "popup")]
    public class ImagePopupTagHelper : TagHelper
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Src { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var id = Guid.NewGuid().ToString();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.Content.SetHtmlContent($@"<a href=""#"" data-toggle=""modal"" data-target=""#{id}""><img src=""{Src}"" width=""{Width}"" height=""{Height}"" /></a>

<div id=""{id}"" class=""modal fade"" tabindex=""-1"" role=""dialog"">
  <div class=""modal-dialog"">
    <div class=""modal-content"">
        <div class=""modal-body"">
            <img src=""{Src}"" class=""img-responsive"">
        </div>
    </div>
  </div>");
        }
    }
}
