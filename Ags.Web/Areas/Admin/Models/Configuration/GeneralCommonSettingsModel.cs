// ***********************************************************************
// Assembly         : Nop.Web
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="GeneralCommonSettingsModel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Configuration
{
    /// <summary>
    /// Represents a general and common settings model
    /// Implements the <see cref="BaseAgsModel" />
    /// Implements the <see cref="ISettingsModel" />
    /// </summary>
    /// <seealso cref="BaseAgsModel" />
    /// <seealso cref="ISettingsModel" />
    public partial class GeneralCommonSettingsModel : BaseAgsModel, ISettingsModel
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralCommonSettingsModel"/> class.
        /// </summary>
        public GeneralCommonSettingsModel()
        {
            StoreInformationSettings = new StoreInformationSettingsModel();

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an active store scope configuration (store identifier)
        /// </summary>
        /// <value>The active store scope configuration.</value>
        public int ActiveStoreScopeConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the store information settings.
        /// </summary>
        /// <value>The store information settings.</value>
        public StoreInformationSettingsModel StoreInformationSettings { get; set; }


        #endregion
    }
}