// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="NewsItem.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Ags.Data.Common;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Customers;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.News
{
    /// <summary>Class NewsItem.
    /// Implements the <see cref="T:Nop.Core.BaseEntity"/>
    /// Implements the <see cref="T:Nop.Core.Domain.Seo.ISlugSupported"/></summary>
    /// <summary>
    /// Class NewsItem.
    /// Implements the <see cref="BaseEntity" />
    /// Implements the <see cref="ISlugSupported" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <seealso cref="ISlugSupported" />
    public partial class NewsItem : BaseEntity, ISlugSupported
    {

        /// <summary>
        /// The category news
        /// </summary>
        private ICollection<CategoryNews> _categoryNews;
        /// <summary>
        /// The news picture mappings
        /// </summary>
        private ICollection<NewsPictureMapping> _newsPictureMappings;
        /// <summary>
        /// The news comments
        /// </summary>
        private ICollection<NewsComment> _newsComments;
        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the short.
        /// </summary>
        /// <value>The short.</value>
        /// <summary>
        /// Gets or sets the short.
        /// </summary>
        /// <value>The short.</value>
        public string Short { get; set; }
        /// <summary>
        /// Gets or sets the full.
        /// </summary>
        /// <value>The full.</value>
        /// <summary>
        /// Gets or sets the full.
        /// </summary>
        /// <value>The full.</value>
        public string Full { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [no title].
        /// </summary>
        /// <value><c>true</c> if [no title]; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether [no title].
        /// </summary>
        /// <value><c>true</c> if [no title]; otherwise, <c>false</c>.</value>
        public bool? NoTitle { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the video identifier.
        /// </summary>
        /// <value>The video identifier.</value>
        /// <summary>
        /// Gets or sets the video identifier.
        /// </summary>
        /// <value>The video identifier.</value>
        public int VideoId { get; set; }
        /// <summary>
        /// Gets or sets the video embed code.
        /// </summary>
        /// <value>The video embed code.</value>
        /// <summary>
        /// Gets or sets the video embed code.
        /// </summary>
        /// <value>The video embed code.</value>
        public string VideoEmbedCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [main banner].
        /// </summary>
        /// <value><c>true</c> if [main banner]; otherwise, <c>false</c>.</value>
        public bool MainBanner { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [last news].
        /// </summary>
        /// <value><c>true</c> if [last news]; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether [last news].
        /// </summary>
        /// <value><c>true</c> if [last news]; otherwise, <c>false</c>.</value>
        public bool LastNews { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [second banner].
        /// </summary>
        /// <value><c>true</c> if [second banner]; otherwise, <c>false</c>.</value>
        public bool SecondBanner { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [up banner].
        /// </summary>
        /// <value><c>true</c> if [up banner]; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether [up banner].
        /// </summary>
        /// <value><c>true</c> if [up banner]; otherwise, <c>false</c>.</value>
        public bool UpBanner { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [flash news].
        /// </summary>
        /// <value><c>true</c> if [flash news]; otherwise, <c>false</c>.</value>
        public bool FlashNews { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NewsItem"/> is approve.
        /// </summary>
        /// <value><c>true</c> if approve; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NewsItem"/> is approve.
        /// </summary>
        /// <value><c>true</c> if approve; otherwise, <c>false</c>.</value>
        public bool Approve { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NewsItem"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NewsItem"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        public bool BigNews { get; set; }
        
        /// <summary>
        /// Gets or sets the start date UTC.
        /// </summary>
        /// <value>The start date UTC.</value>
        public DateTime? StartDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the end date UTC.
        /// </summary>
        /// <value>The end date UTC.</value>
        /// <summary>
        /// Gets or sets the end date UTC.
        /// </summary>
        /// <value>The end date UTC.</value>
        public DateTime? EndDateUtc { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [allow comments].
        /// </summary>
        /// <value><c>true</c> if [allow comments]; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether [allow comments].
        /// </summary>
        /// <value><c>true</c> if [allow comments]; otherwise, <c>false</c>.</value>
        public bool AllowComments { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords.
        /// </summary>
        /// <value>The meta keywords.</value>
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
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        public virtual Video Video { get; set; }
        /// <summary>
        /// Creates new scomments.
        /// </summary>
        /// <value>The news comments.</value>
        public virtual ICollection<NewsComment> NewsComments
        {
            get => _newsComments ?? (_newsComments = new List<NewsComment>());
            protected set => _newsComments = value;
        }
        /// <summary>
        /// Gets or sets the category news.
        /// </summary>
        /// <value>The category news.</value>
        public virtual ICollection<CategoryNews> CategoryNews
        {
            get=>_categoryNews??(_categoryNews=new List<CategoryNews>());
            protected set=>_categoryNews=value;
        }

        /// <summary>
        /// Creates new spicturemappings.
        /// </summary>
        /// <value>The news picture mappings.</value>
        public virtual ICollection<NewsPictureMapping> NewsPictureMappings
        {
            get=>_newsPictureMappings??(_newsPictureMappings=new List<NewsPictureMapping>());
            protected set=>_newsPictureMappings=value;
        }
    }
}
