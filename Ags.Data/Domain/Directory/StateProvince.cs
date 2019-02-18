// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="StateProvince.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Data.Common;

namespace Ags.Data.Domain.Directory
{
    /// <summary>
    /// Class StateProvince.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class StateProvince:BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>The abbreviation.</value>
        public string Abbreviation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="StateProvince"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

    }
}
