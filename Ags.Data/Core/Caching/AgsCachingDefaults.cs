// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 06-24-2018
// ***********************************************************************
// <copyright file="NopCachingDefaults.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Ags.Data.Core.Caching
{
    /// <summary>
    /// Represents default values related to caching
    /// </summary>
    public static partial class AgsCachingDefaults
    {
        /// <summary>
        /// Gets the default cache time in minutes
        /// </summary>
        /// <value>The cache time.</value>
        public static int CacheTime => 60;

        /// <summary>
        /// Gets the key used to store the protection key list to Redis (used with the PersistDataProtectionKeysToRedis option enabled)
        /// </summary>
        /// <value>The redis data protection key.</value>
        public static string RedisDataProtectionKey => "Ags.DataProtectionKeys";
    }
}