// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="EnewsPaper.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.News
{
    /// <summary>
    /// Class EnewsPaper.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class EnewsPaper :BaseEntity,ISlugSupported
    {
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Creates new spapercategoryid.
        /// </summary>
        /// <value>The news paper category identifier.</value>
        public int NewsPaperCategoryId { get; set; }
        /// <summary>
        /// Creates new spapercategory.
        /// </summary>
        /// <value>The news paper category.</value>
        public virtual NewsPaperCategory NewsPaperCategory { get; set; }
        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        public virtual Picture Picture { get; set; }

    }
}
