using System;
using Ags.Data.Core;
using Ags.Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ags.Web.Framework.TagHelpers.Admin
{
    /// <summary>
    /// nop-nested-setting tag helper
    /// </summary>
    [HtmlTargetElement("ags-nested-setting", Attributes = ForAttributeName)]
    public class AgsNestedSettingTagHelper : TagHelper
    {

        private const string ForAttributeName = "asp-for";

        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        /// <param name="adminAreaSettings">Admin area settings</param>
        public AgsNestedSettingTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;

        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            string parentSettingName = For.Name;

            int random = CommonHelper.GenerateRandomInteger();
            string nestedSettingId = $"nestedSetting{random}";
            string parentSettingId = $"parentSetting{random}";

            //tag details
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "nested-setting");
            if (context.AllAttributes.ContainsName("id"))
                nestedSettingId = context.AllAttributes["id"].Value.ToString();

            //use javascript
            if (true)
            {
                TagBuilder script = new TagBuilder("script");
                script.InnerHtml.AppendHtml("$(document).ready(function () {" +
                                                $"initNestedSetting('{parentSettingName}', '{parentSettingId}', '{nestedSettingId}');" +
                                            "});");
                output.PreContent.SetHtmlContent(script.RenderHtmlContent());
            }
        }
    }
}