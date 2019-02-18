// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Advertisement.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;

namespace Ags.Data.Domain.Common
{
    /// <summary>
    /// Class Advertisement.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Advertisement :BaseEntity
    {

        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>

        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the advertising space identifier.
        /// </summary>
        /// <value>The advertising space identifier.</value>
        public int SectionId { get; set; }
        /// <summary>
        /// Gets or sets the target identifier.
        /// </summary>
        /// <value>The target identifier.</value>
        public bool TargetId { get; set; }
        /// <summary>
        /// Gets or sets the code flash.
        /// </summary>
        /// <value>The code flash.</value>
        public string CodeFlash { get; set; }
        /// <summary>
        /// Gets or sets the flash code.
        /// </summary>
        /// <value>The flash code.</value>
        public string FlashCode { get; set; }
        /// <summary>
        /// Gets or sets the URL address.
        /// </summary>
        /// <value>The URL address.</value>
        public string UrlAddress { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        public bool IsApproved { get; set; }

        public virtual Section Section { get; set; }
    }
}
