// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Log.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;
using Ags.Data.Domain.Customers;

namespace Ags.Data.Domain.Logging
{
    /// <summary>
    /// Class Log.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Log  :BaseEntity
    {
        /// <summary>
        /// Gets or sets the log level identifier.
        /// </summary>
        /// <value>The log level identifier.</value>
        public int LogLevelId { get; set; }
        /// <summary>
        /// Gets or sets the short message.
        /// </summary>
        /// <value>The short message.</value>
        public string ShortMessage { get; set; }
        /// <summary>
        /// Gets or sets the full message.
        /// </summary>
        /// <value>The full message.</value>
        public string FullMessage { get; set; }
        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int? CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>The page URL.</value>
        public string PageUrl { get; set; }
        /// <summary>
        /// Gets or sets the referrer URL.
        /// </summary>
        /// <value>The referrer URL.</value>
        public string ReferrerUrl { get; set; }
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the log level
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel LogLevel
        {
            get => (LogLevel)LogLevelId;
            set => LogLevelId = (int)value;
        }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        /// <value>The customer.</value>
        public virtual Customer Customer { get; set; }
    }
}
