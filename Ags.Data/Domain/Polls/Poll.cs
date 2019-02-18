// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Poll.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Ags.Data.Common;

namespace Ags.Data.Domain.Polls
{
    /// <summary>
    /// Class Poll.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class Poll:BaseEntity
    {

        /// <summary>
        /// The poll answers
        /// </summary>
        private ICollection<PollAnswer> _pollAnswers;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the system keyword.
        /// </summary>
        /// <value>The system keyword.</value>
        public string SystemKeyword { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Poll"/> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show on home page].
        /// </summary>
        /// <value><c>true</c> if [show on home page]; otherwise, <c>false</c>.</value>
        public bool ShowOnHomePage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [allow guests to vote].
        /// </summary>
        /// <value><c>true</c> if [allow guests to vote]; otherwise, <c>false</c>.</value>
        public bool AllowGuestsToVote { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the start date UTC.
        /// </summary>
        /// <value>The start date UTC.</value>
        public DateTime? StartDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the end date UTC.
        /// </summary>
        /// <value>The end date UTC.</value>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the poll answers.
        /// </summary>
        /// <value>The poll answers.</value>
        public virtual ICollection<PollAnswer> PollAnswers
        {
            get=>_pollAnswers??(_pollAnswers=new List<PollAnswer>());
            protected set=>_pollAnswers=value;
        }
    }
}
