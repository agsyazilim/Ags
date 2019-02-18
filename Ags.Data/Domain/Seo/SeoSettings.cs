// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="SeoSettings.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain.Seo
{
    /// <summary>
    /// SEO settings
    /// Implements the <see cref="ISettings" />
    /// </summary>
    /// <seealso cref="ISettings" />
    public class SeoSettings : ISettings
    {
        public SeoSettings()
        {
            ReservedUrlRecordSlugs = new List<string>();
        }
        /// <summary>
        /// Page title separator
        /// </summary>
        /// <value>The page title separator.</value>
        public string PageTitleSeparator { get; set; }

        /// <summary>
        /// Page title SEO adjustment
        /// </summary>
        /// <value>The page title seo adjustment.</value>
        public PageTitleSeoAdjustment PageTitleSeoAdjustment { get; set; }

        /// <summary>
        /// Default title
        /// </summary>
        /// <value>The default title.</value>
        public string DefaultTitle { get; set; }

        /// <summary>
        /// Default META keywords
        /// </summary>
        /// <value>The default meta keywords.</value>
        public string DefaultMetaKeywords { get; set; }

        /// <summary>
        /// Default META description
        /// </summary>
        /// <value>The default meta description.</value>
        public string DefaultMetaDescription { get; set; }

        /// <summary>
        /// A value indicating whether product META descriptions will be generated automatically (if not entered)
        /// </summary>
        /// <value><c>true</c> if [generate product meta description]; otherwise, <c>false</c>.</value>
        public bool GenerateProductMetaDescription { get; set; }

        /// <summary>
        /// A value indicating whether we should convert non-western chars to western ones
        /// </summary>
        /// <value><c>true</c> if [convert non western chars]; otherwise, <c>false</c>.</value>
        public bool ConvertNonWesternChars { get; set; }

        /// <summary>
        /// A value indicating whether unicode chars are allowed
        /// </summary>
        /// <value><c>true</c> if [allow unicode chars in urls]; otherwise, <c>false</c>.</value>
        public bool AllowUnicodeCharsInUrls { get; set; }

        /// <summary>
        /// A value indicating whether canonical URL tags should be used
        /// </summary>
        /// <value><c>true</c> if [canonical urls enabled]; otherwise, <c>false</c>.</value>
        public bool CanonicalUrlsEnabled { get; set; }

        /// <summary>
        /// A value indicating whether to use canonical URLs with query string parameters
        /// </summary>
        /// <value><c>true</c> if [query string in canonical urls enabled]; otherwise, <c>false</c>.</value>
        public bool QueryStringInCanonicalUrlsEnabled { get; set; }

        /// <summary>
        /// WWW requires (with or without WWW)
        /// </summary>
        /// <value>The WWW requirement.</value>
        public WwwRequirement WwwRequirement { get; set; }

        /// <summary>
        /// A value indicating whether JS file bundling and minification is enabled
        /// </summary>
        /// <value><c>true</c> if [enable js bundling]; otherwise, <c>false</c>.</value>
        public bool EnableJsBundling { get; set; }

        /// <summary>
        /// A value indicating whether CSS file bundling and minification is enabled
        /// </summary>
        /// <value><c>true</c> if [enable CSS bundling]; otherwise, <c>false</c>.</value>
        public bool EnableCssBundling { get; set; }

        /// <summary>
        /// A value indicating whether Twitter META tags should be generated
        /// </summary>
        /// <value><c>true</c> if [twitter meta tags]; otherwise, <c>false</c>.</value>
        public bool TwitterMetaTags { get; set; }

        /// <summary>
        /// A value indicating whether Open Graph META tags should be generated
        /// </summary>
        /// <value><c>true</c> if [open graph meta tags]; otherwise, <c>false</c>.</value>
        public bool OpenGraphMetaTags { get; set; }

        /// <summary>
        /// Slugs (sename) reserved for some other needs
        /// </summary>
        /// <value>The reserved URL record slugs.</value>
        public List<string> ReservedUrlRecordSlugs { get; set; }

        /// <summary>
        /// Custom tags in the <![CDATA[<head></head>]]> section
        /// </summary>
        /// <value>The custom head tags.</value>
        public string CustomHeadTags { get; set; }
    }
}