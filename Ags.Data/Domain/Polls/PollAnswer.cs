// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="PollAnswer.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain.Polls
{
    /// <summary>
    /// Class PollAnswer.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class PollAnswer :BaseEntity
    {
        /// <summary>
        /// The poll voting records
        /// </summary>
        private ICollection<PollVotingRecord> _pollVotingRecords;

        /// <summary>
        /// Gets or sets the poll identifier.
        /// </summary>
        /// <value>The poll identifier.</value>
        public int PollId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the number of votes.
        /// </summary>
        /// <value>The number of votes.</value>
        public int NumberOfVotes { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the poll.
        /// </summary>
        /// <value>The poll.</value>
        public virtual Poll Poll { get; set; }
        /// <summary>
        /// Gets or sets the poll voting records
        /// </summary>
        /// <value>The poll voting records.</value>
        public virtual ICollection<PollVotingRecord> PollVotingRecords
        {
            get => _pollVotingRecords ?? (_pollVotingRecords = new List<PollVotingRecord>());
            protected set => _pollVotingRecords = value;
        }
    }
}
