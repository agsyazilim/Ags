// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="VideoGallery.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class VideoGallery.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class VideoGallery :BaseEntity
    {
        /// <summary>
        /// The videos
        /// </summary>
        private ICollection<Video> _videos;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VideoGallery"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the videos.
        /// </summary>
        /// <value>The videos.</value>
        public virtual ICollection<Video> Videos
        {
            get=>_videos??(_videos=new List<Video>());
            protected set=>_videos=value;
        }
    }
}
