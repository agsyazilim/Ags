// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Picture.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Data.Common;


namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class Picture.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Picture  :BaseEntity
    {
        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public string MimeType { get; set; }
        /// <summary>
        /// Gets or sets the seo filename.
        /// </summary>
        /// <value>The seo filename.</value>
        public string SeoFilename { get; set; }
        /// <summary>
        /// Gets or sets the alt attribute.
        /// </summary>
        /// <value>The alt attribute.</value>
        public string AltAttribute { get; set; }
        /// <summary>
        /// Gets or sets the title attribute.
        /// </summary>
        /// <value>The title attribute.</value>
        public string TitleAttribute { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        public bool IsNew { get; set; }
        /// <summary>
        /// Gets or sets the picture binary.
        /// </summary>
        /// <value>The picture binary.</value>
        public virtual PictureBinary PictureBinary { get; set; }

    }
}
