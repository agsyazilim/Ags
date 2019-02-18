// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Topic.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Data.Common;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Topics
{
    /// <summary>
    /// Class Topic.
    /// Implements the <see cref="BaseEntity" />
    /// Implements the <see cref="ISlugSupported" />
    /// Implements the <see cref="Nop.Core.Domain.Security.IAclSupported" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <seealso cref="ISlugSupported" />
    /// <seealso cref="Nop.Core.Domain.Security.IAclSupported" />
    public partial class Topic :BaseEntity , ISlugSupported 
    {
        /// <summary>
        /// Gets or sets the name of the system.
        /// </summary>
        /// <value>The name of the system.</value>
        public string SystemName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in sitemap].
        /// </summary>
        /// <value><c>true</c> if [include in sitemap]; otherwise, <c>false</c>.</value>
        public bool IncludeInSitemap { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in top menu].
        /// </summary>
        /// <value><c>true</c> if [include in top menu]; otherwise, <c>false</c>.</value>
        public bool IncludeInTopMenu { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in footer column1].
        /// </summary>
        /// <value><c>true</c> if [include in footer column1]; otherwise, <c>false</c>.</value>
        public bool IncludeInFooterColumn1 { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in footer column2].
        /// </summary>
        /// <value><c>true</c> if [include in footer column2]; otherwise, <c>false</c>.</value>
        public bool IncludeInFooterColumn2 { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in footer column3].
        /// </summary>
        /// <value><c>true</c> if [include in footer column3]; otherwise, <c>false</c>.</value>
        public bool IncludeInFooterColumn3 { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [accessible when store closed].
        /// </summary>
        /// <value><c>true</c> if [accessible when store closed]; otherwise, <c>false</c>.</value>
        public bool AccessibleWhenStoreClosed { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is password protected.
        /// </summary>
        /// <value><c>true</c> if this instance is password protected; otherwise, <c>false</c>.</value>
        public bool IsPasswordProtected { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Topic"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Converts to pictemplateid.
        /// </summary>
        /// <value>The topic template identifier.</value>
        public int TopicTemplateId { get; set; }
        /// <summary>
        /// Gets or sets the meta keywords.
        /// </summary>
        /// <value>The meta keywords.</value>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>The meta title.</value>
        public string MetaTitle { get; set; }
        
    }
}
