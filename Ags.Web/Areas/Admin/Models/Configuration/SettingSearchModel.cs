// ***********************************************************************
// Assembly         : Nop.Web
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="SettingSearchModel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Configuration
{
    /// <summary>
    /// Represents a setting search model
    /// Implements the <see cref="BaseSearchModel" />
    /// </summary>
    /// <seealso cref="BaseSearchModel" />
    public partial class SettingSearchModel : BaseSearchModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the search setting.
        /// </summary>
        /// <value>The name of the search setting.</value>
        [AgsDisplayName("Adına Göre Ara :")]
        public string SearchSettingName { get; set; }

        /// <summary>
        /// Gets or sets the search setting value.
        /// </summary>
        /// <value>The search setting value.</value>
        [AgsDisplayName("Değerine Göre Ara :")]
        public string SearchSettingValue { get; set; }

        #endregion
    }
}