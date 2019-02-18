// ***********************************************************************
// Assembly         : Nop.Web
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 04-05-2018
// ***********************************************************************
// <copyright file="SettingModeModel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Configuration
{
    /// <summary>
    /// Represents a setting mode model
    /// Implements the <see cref="BaseAgsModel" />
    /// </summary>
    /// <seealso cref="BaseAgsModel" />
    public partial class SettingModeModel : BaseAgsModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the mode.
        /// </summary>
        /// <value>The name of the mode.</value>
        public string ModeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SettingModeModel"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        #endregion
    }
}