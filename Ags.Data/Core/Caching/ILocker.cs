// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 07-02-2018
// ***********************************************************************
// <copyright file="ILocker.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Ags.Data.Core.Caching
{
    /// <summary>
    /// Interface ILocker
    /// </summary>
    public interface ILocker
    {
        /// <summary>
        /// Perform some action with exclusive lock
        /// </summary>
        /// <param name="resource">The key we are locking on</param>
        /// <param name="expirationTime">The time after which the lock will automatically be expired</param>
        /// <param name="action">Action to be performed with locking</param>
        /// <returns>True if lock was acquired and action was performed; otherwise false</returns>
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
