using System;
using System.Collections.Generic;
using Ags.Data.Core;
using Ags.Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ags.Web.Framework.TagHelpers.Admin
{
    /// <summary>
    /// ags-editor tag helper
    /// </summary>
    [HtmlTargetElement("ags-editor", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class AgsEditorTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string DisabledAttributeName = "asp-disabled";
        private const string RequiredAttributeName = "asp-required";
        private const string RenderFormControlClassAttributeName = "asp-render-form-control-class";
        private const string TemplateAttributeName = "asp-template";
        private const string PostfixAttributeName = "asp-postfix";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Indicates whether the field is disabled
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsDisabled { set; get; }

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public string IsRequired { set; get; }

        /// <summary>
        /// Indicates whether the "form-control" class shold be added to the input
        /// </summary>
        [HtmlAttributeName(RenderFormControlClassAttributeName)]
        public string RenderFormControlClass { set; get; }

        /// <summary>
        /// Editor template for the field
        /// </summary>
        [HtmlAttributeName(TemplateAttributeName)]
        public string Template { set; get; }

        /// <summary>
        /// Postfix
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        public AgsEditorTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
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

            //clear the output
            output.SuppressOutput();

            //container for additional attributes
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

            //disabled attribute
            bool.TryParse(IsDisabled, out bool disabled);
            if (disabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            //required asterisk
            bool.TryParse(IsRequired, out bool required);
            if (required)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }

            //contextualize IHtmlHelper
            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //add form-control class
            bool.TryParse(RenderFormControlClass, out bool renderFormControlClass);
            if (string.IsNullOrEmpty(RenderFormControlClass) && For.Metadata.ModelType.Name.Equals("String") || renderFormControlClass)
                htmlAttributes.Add("class", "form-control");

            //generate editor

            //we have to invoke strong typed "EditorFor" method of HtmlHelper<TModel>
            //but we cannot do it because we don't have access to Expression<Func<TModel, TValue>>
            //more info at https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/ViewFeatures/HtmlHelperOfT.cs

            //so we manually invoke implementation of "GenerateEditor" method of HtmlHelper
            //more info at https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/ViewFeatures/HtmlHelper.cs

            //little workaround here. we need to access private properties of HtmlHelper
            //just ensure that they are not renamed by asp.net core team in future versions
            IViewEngine viewEngine = CommonHelper.GetPrivateFieldValue(_htmlHelper, "_viewEngine") as IViewEngine;
            IViewBufferScope bufferScope = CommonHelper.GetPrivateFieldValue(_htmlHelper, "_bufferScope") as IViewBufferScope;
            TemplateBuilder templateBuilder = new TemplateBuilder(
                viewEngine,
                bufferScope,
                _htmlHelper.ViewContext,
                _htmlHelper.ViewData,
                For.ModelExplorer,
                For.Name,
                Template,
                readOnly: false,
                additionalViewData: new { htmlAttributes, postfix = this.Postfix });

            Microsoft.AspNetCore.Html.IHtmlContent htmlOutput = templateBuilder.Build();
            output.Content.SetHtmlContent(htmlOutput.RenderHtmlContent());
        }
    }
}