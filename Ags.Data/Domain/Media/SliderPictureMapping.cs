// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="SliderPictureMapping.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Data.Common;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class SliderPictureMapping.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class SliderPictureMapping:BaseEntity
    {
        /// <summary>
        /// Gets or sets the slider identifier.
        /// </summary>
        /// <value>The slider identifier.</value>
        public int SliderId { get; set; }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the picture title.
        /// </summary>
        /// <value>The picture title.</value>
        public string PictureTitle { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        public virtual Picture Picture { get; set; }
        /// <summary>
        /// Gets or sets the slider.
        /// </summary>
        /// <value>The slider.</value>
        public virtual Slider Slider { get; set; }
    }
}
