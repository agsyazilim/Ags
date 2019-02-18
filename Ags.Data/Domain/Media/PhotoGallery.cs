// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="PhotoGallery.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class PhotoGallery.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class PhotoGallery  :BaseEntity
    {

        /// <summary>
        /// The photo gallery mappings
        /// </summary>
        private ICollection<PhotoGalleryMapping> _photoGalleryMappings;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PhotoGallery"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the photo gallery mappings.
        /// </summary>
        /// <value>The photo gallery mappings.</value>
        public virtual ICollection<PhotoGalleryMapping> PhotoGalleryMappings
        {
            get=>_photoGalleryMappings??(_photoGalleryMappings=new List<PhotoGalleryMapping>());
            protected set => _photoGalleryMappings = value;
        }
    }
}
