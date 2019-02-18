// ***********************************************************************
// Assembly         : Nop.Web
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="SettingModel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Configuration
{
    /// <summary>
    /// Represents a setting model
    /// Implements the <see cref="BaseAgsEntityModel" />
    /// </summary>
    /// <seealso cref="BaseAgsEntityModel" />
    public partial class SettingModel : BaseAgsEntityModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [AgsDisplayName("Adı :")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [AgsDisplayName("Değeri :")]
        [Required]
        public string Value { get; set; }


        /// <summary>
        /// Gets or sets the store identifier.
        /// </summary>
        /// <value>The store identifier.</value>
        public int StoreId { get; set; }

        #endregion
    }
}