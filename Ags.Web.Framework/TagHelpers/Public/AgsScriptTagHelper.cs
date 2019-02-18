using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ags.Web.Framework.TagHelpers.Public
{
    [HtmlTargetElement("script", Attributes = "on-content-loaded")]
    public class AgsScriptTagHelper : TagHelper
    {
        public bool OnContentLoaded { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!OnContentLoaded)
            {
                base.Process(context, output);
            }
            else
            {
                var content = output.GetChildContentAsync().Result;
                var javascript = content.GetContent();

                var sb = new StringBuilder();
                sb.Append("document.addEventListener('DOMContentLoaded',");
                sb.Append("function() {");
                sb.Append(javascript);
                sb.Append("});");

                output.Content.SetHtmlContent(sb.ToString());
            }
        }
    }
}