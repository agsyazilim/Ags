// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Company.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;
using Ags.Data.Domain.Directory;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Catalog
{
    /// <summary>
    /// Class Company.
    /// Implements the <see cref="BaseEntity" />
    /// Implements the <see cref="Nop.Core.Domain.Security.IAclSupported" />
    /// Implements the <see cref="ISlugSupported" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <seealso cref="Nop.Core.Domain.Security.IAclSupported" />
    /// <seealso cref="ISlugSupported" />
    public partial class Company :BaseEntity ,ISlugSupported
    {

        /// <summary>
        /// Gets or sets the company category identifier.
        /// </summary>
        /// <value>The company category identifier.</value>
        public int CompanyCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the GSM.
        /// </summary>
        /// <value>The GSM.</value>
        public string Gsm { get; set; }
        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>The fax.</value>
        public string Fax { get; set; }
        /// <summary>
        /// Gets or sets the WWW.
        /// </summary>
        /// <value>The WWW.</value>
        public string Www { get; set; }
        /// <summary>
        /// Gets or sets the state provence identifier.
        /// </summary>
        /// <value>The state provence identifier.</value>
        public int StateProvincesId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Company"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets the video embed code.
        /// </summary>
        /// <value>The video embed code.</value>
        public string VideoEmbedCode { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PictureId { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Gets or sets the company categories.
        /// </summary>
        /// <value>The company categories.</value>
        public virtual CompanyCategory CompanyCategories { get; set; }
        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        /// <value>The picture.</value>
        public virtual Picture Picture { get; set; }

        public virtual StateProvince StateProvinces { get; set; }


    }
}
