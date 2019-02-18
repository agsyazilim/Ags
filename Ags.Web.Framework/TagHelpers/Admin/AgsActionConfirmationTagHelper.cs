﻿using System;
using System.Threading.Tasks;
using Ags.Web.Framework.Extensions;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ags.Web.Framework.TagHelpers.Admin
{
    /// <summary>
    /// nop-action-confirmation tag helper
    /// </summary>
    [HtmlTargetElement("ags-action-confirmation", Attributes = ButtonIdAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class AgsActionConfirmationTagHelper : TagHelper
    {
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string ActionAttributeName = "asp-action";
        private const string AdditionaConfirmText = "asp-additional-confirm";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// Button identifier
        /// </summary>
        [HtmlAttributeName(ButtonIdAttributeName)]
        public string ButtonId { get; set; }

        /// <summary>
        /// Delete action name
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Additional confirm text
        /// </summary>
        [HtmlAttributeName(AdditionaConfirmText)]
        public string ConfirmText { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        /// <param name="htmlHelper">HTML helper</param>
        public AgsActionConfirmationTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper)
        {
            Generator = generator;
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //contextualize IHtmlHelper
            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            if (string.IsNullOrEmpty(Action))
                Action = _htmlHelper.ViewContext.RouteData.Values["action"].ToString();

            string modalId = new HtmlString(ButtonId + "-action-confirmation").ToHtmlString();

            ActionConfirmationModel actionConfirmationModel = new ActionConfirmationModel()
            {
                ControllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString(),
                ActionName = Action,
                WindowId = modalId,
                AdditonalConfirmText = ConfirmText
            };

            //tag details
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", modalId);
            output.Attributes.Add("class", "modal fade");
            output.Attributes.Add("tabindex", "-1");
            output.Attributes.Add("role", "dialog");
            output.Attributes.Add("aria-labelledby", $"{modalId}-title");
            output.Content.SetHtmlContent(await _htmlHelper.PartialAsync("Confirm", actionConfirmationModel));

            //modal script
            TagBuilder script = new TagBuilder("script");
            script.InnerHtml.AppendHtml("$(document).ready(function () {" +
                                        $"$('#{ButtonId}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{modalId}\");" +
                                        $"$('#{modalId}-submit-button').attr(\"name\", $(\"#{ButtonId}\").attr(\"name\"));" +
                                        $"$(\"#{ButtonId}\").attr(\"name\", \"\");" +
                                        $"if($(\"#{ButtonId}\").attr(\"type\") == \"submit\")$(\"#{ButtonId}\").attr(\"type\", \"button\");" +
                                        "});");
            output.PostContent.SetHtmlContent(script.RenderHtmlContent());
        }
    }
}