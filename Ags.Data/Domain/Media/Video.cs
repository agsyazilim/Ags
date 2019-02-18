// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Video.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Data.Common;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class Video.
    /// Implements the <see cref="BaseEntity" />
    /// Implements the <see cref="ISlugSupported" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <seealso cref="ISlugSupported" />
    public partial class Video:BaseEntity ,ISlugSupported
    {

        /// <summary>
        /// Gets or sets the video gallery identifier.
        /// </summary>
        /// <value>The video gallery identifier.</value>
        public int VideoGalleryId { get; set; }
        /// <summary>
        /// Video Resim Yükleme
        /// </summary>
        public int PictureId { get; set; }
        /// <summary>
        /// Resim Url
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the descriptions.
        /// </summary>
        /// <value>The descriptions.</value>
        public string Descriptions { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Video"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the embed code.
        /// </summary>
        /// <value>The embed code.</value>
        public string EmbedCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the video gallery.
        /// </summary>
        /// <value>The video gallery.</value>
        public virtual VideoGallery VideoGallery { get; set; }
    }
}
