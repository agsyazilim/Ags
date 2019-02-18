// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Download.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class Download.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Download :BaseEntity
    {
        /// <summary>
        /// Gets or sets the download GUID.
        /// </summary>
        /// <value>The download GUID.</value>
        public Guid DownloadGuid { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use download URL].
        /// </summary>
        /// <value><c>true</c> if [use download URL]; otherwise, <c>false</c>.</value>
        public bool UseDownloadUrl { get; set; }
        /// <summary>
        /// Gets or sets the download URL.
        /// </summary>
        /// <value>The download URL.</value>
        public string DownloadUrl { get; set; }
        /// <summary>
        /// Gets or sets the download binary.
        /// </summary>
        /// <value>The download binary.</value>
        public byte[] DownloadBinary { get; set; }
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        public string Filename { get; set; }
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        public string Extension { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is new.
        /// </summary>
        /// <value><c>true</c> if this instance is new; otherwise, <c>false</c>.</value>
        public bool IsNew { get; set; }
    }
}
