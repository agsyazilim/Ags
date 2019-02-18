// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Slider.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Ags.Data.Common;
using Ags.Data.Domain.Common;

namespace Ags.Data.Domain.Media
{
    /// <summary>
    /// Class Slider.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Slider:BaseEntity
    {
        private  ICollection<SliderPictureMapping> _sliderPictureMappings;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>The section identifier.</value>
        public int SectionId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Slider"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime CreateDate { get; set; }

        public virtual Section Section { get; set; }

        public virtual ICollection<SliderPictureMapping> SliderPictureMappings
        {
            get => _sliderPictureMappings ?? (_sliderPictureMappings = new List<SliderPictureMapping>());

            protected set => _sliderPictureMappings = value;
        }

    }
}
