// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Section.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using Ags.Data.Common;

namespace Ags.Data.Domain.Common
{
    /// <summary>
    /// Class Section.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Section:BaseEntity
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DisplayName("Sayfa Bölgeleri")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the decription.
        /// </summary>
        /// <value>The decription.</value>
        [DisplayName("Açıklama")]
        public string Decription { get; set; }
    }
}
