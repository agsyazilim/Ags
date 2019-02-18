// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="ScheduleTask.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;

namespace Ags.Data.Domain.Tasks
{
    /// <summary>
    /// Class ScheduleTask.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class ScheduleTask  :BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the seconds.
        /// </summary>
        /// <value>The seconds.</value>
        public int Seconds { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ScheduleTask"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [stop on error].
        /// </summary>
        /// <value><c>true</c> if [stop on error]; otherwise, <c>false</c>.</value>
        public bool StopOnError { get; set; }
        /// <summary>
        /// Gets or sets the last start UTC.
        /// </summary>
        /// <value>The last start UTC.</value>
        public DateTime? LastStartUtc { get; set; }
        /// <summary>
        /// Gets or sets the last end UTC.
        /// </summary>
        /// <value>The last end UTC.</value>
        public DateTime? LastEndUtc { get; set; }
        /// <summary>
        /// Gets or sets the last success UTC.
        /// </summary>
        /// <value>The last success UTC.</value>
        public DateTime? LastSuccessUtc { get; set; }
    }
}
