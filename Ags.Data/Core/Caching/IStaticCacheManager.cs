// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 07-02-2018
// ***********************************************************************
// <copyright file="IStaticCacheManager.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Ags.Data.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// Implements the <see cref="ICacheManager" />
    /// </summary>
    /// <seealso cref="ICacheManager" />
    public interface IStaticCacheManager : ICacheManager
    {
    }
}