// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Store.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using Ags.Data.Common;

namespace Ags.Data.Domain.Stores
{
    /// <summary>
    /// Class Store.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Store :BaseEntity
    {


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DisplayName("Site Adı")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [DisplayName("Url Adresi")]
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [SSL enabled].
        /// </summary>
        /// <value><c>true</c> if [SSL enabled]; otherwise, <c>false</c>.</value>
        [DisplayName("SSL Durumu")]
        public bool SslEnabled { get; set; }
        /// <summary>
        /// Gets or sets the hosts.
        /// </summary>
        /// <value>The hosts.</value>
        [DisplayName("Hosting")]
        public string Hosts { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        [DisplayName("Sırası")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        [DisplayName("Firma Adı")]
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>The company address.</value>
        [DisplayName("Adresi")]
        public string CompanyAddress { get; set; }
        /// <summary>
        /// Gets or sets the company phone number.
        /// </summary>
        /// <value>The company phone number.</value>
        [DisplayName("Telefon")]
        public string CompanyPhoneNumber { get; set; }


    }
}
