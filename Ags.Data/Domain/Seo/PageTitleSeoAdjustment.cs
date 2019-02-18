// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="PageTitleSeoAdjustment.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Ags.Data.Domain.Seo
{
    /// <summary>
    /// Represents a page title SEO adjustment
    /// </summary>
    public enum PageTitleSeoAdjustment
    {
        /// <summary>
        /// Pagename comes after storename
        /// </summary>
        PagenameAfterStorename = 0,

        /// <summary>
        /// Storename comes after pagename
        /// </summary>
        StorenameAfterPagename = 10
    }
}
