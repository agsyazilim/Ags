// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="PollVotingRecord.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;
using Ags.Data.Domain.Customers;

namespace Ags.Data.Domain.Polls
{
    /// <summary>
    /// Class PollVotingRecord.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class PollVotingRecord :BaseEntity
    {
        /// <summary>
        /// Gets or sets the poll answer identifier.
        /// </summary>
        /// <value>The poll answer identifier.</value>
        public int PollAnswerId { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the poll answer.
        /// </summary>
        /// <value>The poll answer.</value>
        public virtual PollAnswer PollAnswers { get; set; }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Customer Customer { get; set; }
    }
}
