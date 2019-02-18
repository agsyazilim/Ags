// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="LogLevel.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Ags.Data.Domain.Logging
{
    /// <summary>
    /// Represents a log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 10,

        /// <summary>
        /// Information
        /// </summary>
        Information = 20,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 30,

        /// <summary>
        /// Error
        /// </summary>
        Error = 40,

        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 50
    }
}
