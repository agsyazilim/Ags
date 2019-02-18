// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Modules.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using Ags.Data.Common;

namespace Ags.Data.Domain.Common
{
    /// <summary>
    /// Class Modules.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Modules :BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DisplayName("Modul Adı")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Modules"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        [DisplayName("Yayında mı?")]
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>The section identifier.</value>
        [DisplayName("Bölge seçin")]
        public int SectionId { get; set; }

        public virtual Section Section { get; set; }
    }
}
