// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="WwwRequirement.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Ags.Data.Domain.Seo
{
    /// <summary>
    /// Represents WWW requirement
    /// </summary>
    public enum WwwRequirement
    {
        /// <summary>
        /// Doesn't matter (do nothing)
        /// </summary>
        NoMatter = 0,

        /// <summary>
        /// Pages should have WWW prefix
        /// </summary>
        WithWww = 10,

        /// <summary>
        /// Pages should not have WWW prefix
        /// </summary>
        WithoutWww = 20
    }
}
